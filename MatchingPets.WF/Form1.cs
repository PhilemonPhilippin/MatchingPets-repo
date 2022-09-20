namespace MatchingPets.WF
{
    public partial class Form1 : Form
    {
        List<int> numbers = new()
        {
            1,1,2,2,3,3,4,4,5,5,6,6
        };
        string firstChoice;
        string secondChoice;
        int tries;
        List<PictureBox> pictures = new();
        PictureBox pictureOne;
        PictureBox pictureTwo;
        int totalTime = 30;
        int countDownTime;
        bool gameOver;

        public Form1()
        {
            InitializeComponent();
            LoadPictures();
        }

        private void TimerEvent(object sender, EventArgs e)
        {
            countDownTime--;

            lblTimeLeft.Text = $"Time left: {countDownTime}";

            if (countDownTime < 1)
            {
                GameOver("Times up, you lose. ");

                foreach (PictureBox picture in pictures)
                {
                    if (picture.Tag is not null)
                    {
                        picture.Image = Image.FromFile($"pics/{(string)picture.Tag}.png");
                    }
                }
            }
        }

        private void RestartGameEvent(object sender, EventArgs e)
        {
            RestartGame();
        }

        private void LoadPictures()
        {
            int leftPosition = 20;
            int topPosition = 20;
            int rows = 0;

            for (int i = 0; i < 12; i++)
            {
                PictureBox newPicture = new();
                newPicture.Height = 50;
                newPicture.Width = 50;
                newPicture.BackColor = Color.LightGray;
                newPicture.SizeMode = PictureBoxSizeMode.StretchImage;
                newPicture.Click += NewPic_Click;
                pictures.Add(newPicture);

                if (rows < 3)
                {
                    rows++;
                    newPicture.Left = leftPosition;
                    newPicture.Top = topPosition;
                    Controls.Add(newPicture);
                    leftPosition = leftPosition + 60;
                }

                if (rows == 3)
                {
                    leftPosition = 20;
                    topPosition += 60;
                    rows = 0;
                }
            }
            RestartGame();
        }

        private void NewPic_Click(object sender, EventArgs e)
        {
            if (gameOver)
            {
                return;
            }
            
            if (firstChoice is null)
            {
                pictureOne = sender as PictureBox;
                if (pictureOne.Tag is not null && pictureOne.Image is null)
                {
                    pictureOne.Image = Image.FromFile($"pics/{(string)pictureOne.Tag}.png");
                    firstChoice = (string)pictureOne.Tag;
                }
            }
            else if (secondChoice is null)
            {
                pictureTwo = sender as PictureBox;

                if (pictureTwo.Tag is not null && pictureTwo.Image is null)
                {
                    pictureTwo.Image = Image.FromFile($"pics/{(string)pictureTwo.Tag}.png");
                    secondChoice = (string)pictureTwo.Tag;
                }
            }
            else
            {
                CheckPictures(pictureOne, pictureTwo);
            }
        }

        private void RestartGame()
        {
            List<int> randomList = numbers.OrderBy(x => Guid.NewGuid()).ToList();
            numbers = randomList;

            for (int i = 0; i < pictures.Count; i++)
            {
                pictures[i].Image = null;
                pictures[i].Tag = numbers[i].ToString();
            }

            tries = 0;
            lblStatus.Text = $"Mismatched: {tries} times.";
            lblTimeLeft.Text = $"Time left: {totalTime}";
            gameOver = false;
            GameTimer.Start();
            countDownTime = totalTime;
        }

        private void CheckPictures(PictureBox PictureOne, PictureBox PictureTwo)
        {
            if (firstChoice == secondChoice)
            {
                PictureOne.Tag = null;
                PictureTwo.Tag = null;
            }
            else
            {
                tries++;
                lblStatus.Text = $"Mismatched: {tries} times.";
            }

            firstChoice = null;
            secondChoice = null;

            foreach (PictureBox picture in pictures.ToList())
            {
                if (picture.Tag is not null)
                {
                    picture.Image = null;
                }
            }

            if (pictures.All(pict => pict.Tag == pictures[0].Tag))
            {
                GameOver("Great work, You Win!!");
            }
        }

        private void GameOver(string msg)
        {
            GameTimer.Stop();
            gameOver = true;
            MessageBox.Show($"{msg} Click Restart to Play Again", "Tarcacode Says: ");
        }
    }
}