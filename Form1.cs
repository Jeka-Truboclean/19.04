using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace _19._04
{
    public partial class Form1 : Form
    {
        private Label questionLabel;
        private TextBox answerTextBox;
        private Button checkAnswerButton;
        private ComboBox levelComboBox;

        private readonly Random _random = new Random();
        private int _currentQuestionIndex = -1;

        private string[] selectedQuestion;
        private readonly string[] _easyQuastion = { "2 + 2", "10 - 5", "3 * 4", "15 / 3" };
        private readonly string[] _mediumQuastion = { "235 * 3", "10.3 / 5.5", "325 * 4.1", "15 / Min(5,3)" };
        private readonly string[] _hardQuastion = { "Sqrt(5 * 5) - Max(5,10) + Pow(2,2)", "4556 * 7", "3 * Pow(2,10)", "15 / Cos(3.3)" };

        public Form1()
        {
            InitializeComponent();
            InitializeControls();
            ShowNextQuestion();
            (Width, Height) = (270, 250);
        }

        private void InitializeControls()
        {
            selectedQuestion = _easyQuastion;

            levelComboBox = new ComboBox()
            {
                Location = new Point(20,20),
                Width = 200
            };
            levelComboBox.Items.AddRange(new string[]
            {
                "Easy", "Medium", "Hard"
            });
            levelComboBox.SelectedIndex = 0;
            levelComboBox.SelectedIndexChanged += LevelComboBox_SelectedIndexChanged;

            questionLabel = new Label()
            {
                Text = "Question",
                Location = new Point(20,70),
                AutoSize = true
            };

            answerTextBox = new TextBox()
            {
                Location = new Point(20,100),
                Width = 200
            };

            checkAnswerButton = new Button()
            {
                Text = "Check the answer",
                Location = new Point(20,130),
                Width = 200
            };

            checkAnswerButton.Click += btnCheckAnswer_Click;

            Controls.Add(levelComboBox);
            Controls.Add(questionLabel);
            Controls.Add(answerTextBox);
            Controls.Add(checkAnswerButton);
        }

        private void LevelComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            switch (levelComboBox.SelectedIndex)
            {
                case 0: { selectedQuestion = _easyQuastion; break; }
                case 1: { selectedQuestion = _mediumQuastion; break; }
                case 2: { selectedQuestion = _hardQuastion; break; }
            }

            questionLabel.Text = selectedQuestion[_currentQuestionIndex];
            _currentQuestionIndex = 0;
            answerTextBox.Text = "";
        }

        private void ShowNextQuestion()
        {
            _currentQuestionIndex++;
            if (_currentQuestionIndex >= selectedQuestion.Length)
            {
                MessageBox.Show("Test completed!");
                return; // ?
            }

            questionLabel.Text = selectedQuestion[_currentQuestionIndex];
            answerTextBox.Text = "";
        }

        private async void btnCheckAnswer_Click(object? sender, EventArgs e)
        {
            int userAnswer;
            if (!int.TryParse(answerTextBox.Text, out userAnswer))
            {
                MessageBox.Show("Enter a numerical answer.");
                return;
            }
            var result = await CSharpScript.EvaluateAsync<double>(questionLabel.Text, ScriptOptions.Default.WithImports("System.Math"));

            if (userAnswer == result)
            {
                MessageBox.Show("That's right!");
                ShowNextQuestion();
            }
            else
            {
                MessageBox.Show("Wrong.");
            }
        }
    }
}
