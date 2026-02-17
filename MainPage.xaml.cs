using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace MonAppMaui;
public partial class MainPage : ContentPage
{
	// ====== VARIABLES ET INITIALISATION ======
	double nb1 = 0;
	double nb2 = 0;
	double result = 0;
	char operation = ' ';
	bool justCalculated = false;
	private ObservableCollection<string> history = new ObservableCollection<string>();

	public MainPage()
	{
		InitializeComponent();
		HistoryCollectionView.ItemsSource = history;
	}


	// ====== AFFICHAGE DES NOMBRES ======
	public void DisplayNumber(string number)
	{
		if (justCalculated)
		{
			TB_Display.Text = number;
			justCalculated = false;
		}
		else if (TB_Display.Text == "")
		{
			TB_Display.Text = number;
		}
		else
		{
			TB_Display.Text += number;
		}
	}

	// ====== BOUTONS DE CHIFFRES (0-9) ======
	private void BTN_1_Click(object sender, EventArgs e)
	{
   		DisplayNumber("1");
	}

	private void BTN_2_Click(object sender, EventArgs e)
	{
    	DisplayNumber("2");
	}

	private void BTN_3_Click(object sender, EventArgs e)
	{
		DisplayNumber("3");
	}

	private void BTN_4_Click(object sender, EventArgs e)
	{
		DisplayNumber("4");
	}

	private void BTN_5_Click(object sender, EventArgs e)
	{
		DisplayNumber("5");
	}

	private void BTN_6_Click(object sender, EventArgs e)
	{
		DisplayNumber("6");
	}

	private void BTN_7_Click(object sender, EventArgs e)
	{
		DisplayNumber("7");
	}

	private void BTN_8_Click(object sender, EventArgs e)
	{
		DisplayNumber("8");
	}

	private void BTN_9_Click(object sender, EventArgs e)
	{
		DisplayNumber("9");
	}

	private void BTN_0_Click(object sender, EventArgs e)
	{
		DisplayNumber("0");
	}

	// ====== OPÉRATIONS ARITHMÉTIQUES (+, -, *, /) ======
	private void BTN_plus_Click(object sender, EventArgs e)
	{
		HandleOperation('+');
	}

	private void BTN_minus_Click(object sender, EventArgs e)
	{
		HandleOperation('-');
	}

	private void BTN_multiply_Click(object sender, EventArgs e)
	{
		HandleOperation('*');
	}

	private void BTN_divide_Click(object sender, EventArgs e)
	{
		HandleOperation('/');
	}

	private void HandleOperation(char newOperation)
	{
		try
		{
			string currentText = TB_Display.Text.Trim();
			
			if (currentText == "" || currentText == "Erreur")
				return;
			
			// Si une opération est déjà en cours (on a "nb1 op nb2" et on appuie sur un autre op)
			if (operation != ' ')
			{
				// Extraire nb2 (le dernier nombre)
				string[] parts = currentText.Split(new[] { " + ", " - ", " × ", " ÷ " }, StringSplitOptions.None);
				if (parts.Length >= 2)
				{
					nb2 = double.Parse(parts[parts.Length - 1]);
					CalculateResult();
					nb1 = result;
					TB_Display.Text = FormatNumber(result) + GetOperatorSymbol(newOperation);
				}
			}
			else
			{
				// Premier opérateur, enregistrer nb1
				nb1 = double.Parse(currentText);
				TB_Display.Text = currentText + GetOperatorSymbol(newOperation);
			}
			
			operation = newOperation;
			justCalculated = false;
		}
		catch
		{
			TB_Display.Text = "Erreur";
		}
	}

	private string GetOperatorSymbol(char op)
	{
		return op switch
		{
			'+' => " + ",
			'-' => " - ",
			'*' => " × ",
			'/' => " ÷ ",
			_ => ""
		};
	}

	private string FormatNumber(double num)
	{
		// Format avec max 10 décimales et enlever les zéros inutiles
		return num.ToString("G10");
	}

	// ====== BOUTONS SPÉCIAUX (Point, Clear, Égal) ======
	private void BTN_point_Click(object sender, EventArgs e)
	{
		string text = TB_Display.Text;
		
		// Si on vient de calculer, commencer par "0."
		if (justCalculated)
		{
			TB_Display.Text = "0.";
			justCalculated = false;
			return;
		}
		
		// Vérifier s'il y a déjà un point dans le nombre actuel
		string lastNumber = "";
		if (text.Contains(" + ") || text.Contains(" - ") || text.Contains(" × ") || text.Contains(" ÷ "))
		{
			// Extraire le dernier nombre (après l'opérateur)
			string[] parts = text.Split(new[] { " + ", " - ", " × ", " ÷ " }, StringSplitOptions.None);
			lastNumber = parts[parts.Length - 1];
		}
		else
		{
			lastNumber = text;
		}
		
		// Si le dernier nombre n'a pas de point, l'ajouter
		if (!lastNumber.Contains("."))
		{
			if (text == "")
			{
				TB_Display.Text = "0.";
			}
			else
			{
				TB_Display.Text += ".";
			}
		}
	}

	private void BTN_CLR_Click(object sender, EventArgs e)
	{
		TB_Display.Text = "";
		nb1 = 0;
		nb2 = 0;
		result = 0;
		operation = ' ';
		justCalculated = false;
	}

	private void BTN_equals_Click(object sender, EventArgs e)
	{
		try
		{
			if (operation == ' ')
				return;
			
			string currentText = TB_Display.Text.Trim();
			
			// Extraire nb2 en splittant par l'opérateur
			string[] parts = currentText.Split(new[] { " + ", " - ", " × ", " ÷ " }, StringSplitOptions.None);
			if (parts.Length < 2)
				return;
			
			nb2 = double.Parse(parts[parts.Length - 1]);
			
			CalculateResult();
			
			TB_Display.Text = FormatNumber(result);
			history.Insert(0, $"{nb1} {GetOperationSymbol()} {nb2} = {FormatNumber(result)}");
			
			operation = ' ';
			justCalculated = true;
		}
		catch
		{
			TB_Display.Text = "Erreur";
		}
	}

	private void CalculateResult()
	{
		switch (operation)
		{
			case '+':
				result = nb1 + nb2;
				break;
			case '-':
				result = nb1 - nb2;
				break;
			case '*':
				result = nb1 * nb2;
				break;
			case '/':
				result = nb1 / nb2;
				break;
		}
	}

	private string GetOperationSymbol()
	{
		return operation switch
		{
			'+' => "+",
			'-' => "-",
			'*' => "×",
			'/' => "÷",
			_ => ""
		};
	}

	// ====== FONCTIONS TRIGONOMÉTRIQUES (Sin, Tan, Cos) ======
	private void BTN_Sin_Click(object sender, EventArgs e)
	{
		try
		{
			double degrees = double.Parse(TB_Display.Text);
			double radians = degrees * Math.PI / 180;
			double sinValue = Math.Sin(radians);
			TB_Display.Text = FormatNumber(sinValue);
			history.Insert(0, $"sin({degrees}°) = {FormatNumber(sinValue)}");
			operation = ' ';
			justCalculated = true;
		}
		catch
		{
			TB_Display.Text = "Erreur";
		}
	}	

	private void BTN_Tan_Click(object sender, EventArgs e)
	{
		try
		{
			double degrees = double.Parse(TB_Display.Text);
			double radians = degrees * Math.PI / 180;
			double tanValue = Math.Tan(radians);
			TB_Display.Text = FormatNumber(tanValue);
			history.Insert(0, $"tan({degrees}°) = {FormatNumber(tanValue)}");
			operation = ' ';
			justCalculated = true;
		}
		catch
		{
			TB_Display.Text = "Erreur";
		}
	}		
			
	private void BTN_Cos_Click(object sender, EventArgs e)
	{
		try
		{
			double degrees = double.Parse(TB_Display.Text);
			double radians = degrees * Math.PI / 180;
			double cosValue = Math.Cos(radians);
			TB_Display.Text = FormatNumber(cosValue);
			history.Insert(0, $"cos({degrees}°) = {FormatNumber(cosValue)}");
			operation = ' ';
			justCalculated = true;
		}
		catch
		{
			TB_Display.Text = "Erreur";
		}
	}

	// ====== POURCENTAGE ======
	private void BTN_Percent_Click(object sender, EventArgs e)
	{
		try
		{
			double value = double.Parse(TB_Display.Text);
			double percentValue = value / 100;
			TB_Display.Text = FormatNumber(percentValue);
			history.Insert(0, $"{value}% = {FormatNumber(percentValue)}");
			operation = ' ';
			justCalculated = true;
		}
		catch
		{
			TB_Display.Text = "Erreur";
		}
	}

	// ====== RACINE CARRÉE ======
	private void BTN_Sqrt_Click(object sender, EventArgs e)
	{
		try
		{
			double value = double.Parse(TB_Display.Text);
			double sqrtValue = Math.Sqrt(value);
			TB_Display.Text = FormatNumber(sqrtValue);
			history.Insert(0, $"√{value} = {FormatNumber(sqrtValue)}");
			operation = ' ';
			justCalculated = true;
		}
		catch
		{
			TB_Display.Text = "Erreur";
		}
	}

	// ====== HISTORIQUE ======
	private void BTN_ClearHistory_Click(object sender, EventArgs e)
{
    history.Clear();
	HistoryCollectionView.ItemsSource = history;
}
}