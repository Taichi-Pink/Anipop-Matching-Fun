# Anipop-Matching-Fun

## Introduction
The fun game is a classic puzzle games. After exchanging two squares, eliminate the continuous same 3 squares in horizontal or vertical,
if the 3 squares are not same, exchange is invalid. According to the number of eliminated squares, increase the corresponding gold coins and accumulated points. 
The gold conins can buy specific skills. When the game time runs out, the game will end and a gameover dialog will pop up.

<img src="https://github.com/Taichi-Pink/Anipop-Matching-Fun/blob/master/img/img.png" width="430"> 


## System Design
### System Function
1. Build Map
* Define a function that initializes the map array, assigning values to each square.
```
 public void BuildMap()
        {
            Random myRand = new Random(DateTime.Now.Second);
            for (int i = 1; i <= _height; i++)
                for (int j = 1; j <= _width; j++)
                {
                    _map[i, j] = new Item((itemtype)myRand.Next(1, 7), new Point(_topleft.X + (j - 1) * _size, _topleft.Y + (i - 1) * _size), _size, 0);
                }
        }
```
* Define a function to draw through the ```Graphics``` class.
```
public void DrawMap(Graphics g)
        {
            for (int i = 1; i <= _height; i++)
                for (int j = 1; j <= _width; j++)
                {
                    _map[i, j].Drawme(g);
                }
        }
```
2. Execute the drop and determine whether it is still available.
* Put the bottom blank into the top of each column, and then replace the top blank by randomly generated map.  
 If there's no blank set ```timer3.Enabled = false```, stop executing the drop.
```
        private void timer3_Tick(object sender, EventArgs e)
        {
            //Search each column and slowly replace the bottom blank, similar to bubble sort
            for (int j = 1; j <= _width; j++)
            {
                for (int i = _height; i >= 2; i--)
                {
                    if (_map[i, j]._Color == itemtype.无)
                    {
                        for (int k = i; k >= 2; k--)
                            swap(k, j, k - 1, j);
                        break;
                    }
                }
            }
            
            // Randomly generate the first blank line.
            Random myRand = new Random();
            for (int j = 1; j <= _width; j++)
                if (_map[1, j]._Color == itemtype.无)
                    _map[1, j]._Color = (itemtype)myRand.Next(1, 7);
            pictureBox1.Invalidate();
            int flag = 0; // Determine still can fall
            for (int i = 1; i <= _height; i++)
            {
                for (int j = 1; j <= _width; j++)
                {
                    if (_map[i, j]._Color == itemtype.无)
                    {
                        flag = 1;
                        break;
                    }
                }
                if (flag == 1)
                    break;
            }
            if (flag == 0) //If can't fall
            {
                if (!clean()) //Determine if still can eliminate; if not, fall ends.
                    timer3.Enabled = false;
            }
        }
```
3. Eliminate
* Search each square towards the lower right, once more than 3 of the same square,
set  ```willclean=true```. 
* Statistical ```tot```, which means how many blocks are eliminated.
* Eliminate ```willclean = true``` square: change the image of the square into a dark blue background image and then 
wait for falling. According to ```tot```, increase gold coins.

4. Swap process
* Swap the images of two squares.
```
 public void swap(int x, int y, int tox, int toy)
        {
            itemtype color = _map[x, y]._Color;
            _map[x, y]._Color = _map[tox, toy]._Color;
            _map[tox, toy]._Color = color;

        }
```
* Gets the row and column index of the squares to swap based on where you clicked.
```
        public bool ConvertPointToRowCol(Point point, out int row, out int col)
        {
            int tempRow = (point.Y - _topleft.Y) / _size + 1;
            int tempCol = (point.X - _topleft.X) / _size + 1;
            if ((tempRow <= 10) && (tempRow >= 1) && (tempCol <= 8) && (tempCol >= 1))
            {
                row = tempRow;
                col = tempCol;
                return true;
            }
            else
            {
                //click invalid
                row = -1;
                col = -1;
                return false;
            }
        }          
```
5. Purchase skills. 
```
        private void buybutton1_Click(object sender, EventArgs e)
        {
            if (_istart == 1 && _stop == 0)
            {
                timer1.Enabled = false;
                timelabel.Text = "Pause";
                if (MessageBox.Show("Are you sure you're going to pay 40 gold pieces for a '***'？", "Buy", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    if (_money >= 40)
                    {
                        _money -= 40;
                        _skill1 += 1;
                        skillbutton1.Text = "Use(" + _skill1.ToString() + ")";
                    }
                    else
                    {
                        MessageBox.Show("Sorry, you don't have enough money~", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                money1.Text = _money.ToString();
                timer1.Enabled = true;
            }
        }
```
6.Pause game
```
 private void stopbutton_Click(object sender, EventArgs e)
        {
            if (_istart == 1)
            {
                if (_stop == 0)
                {
                    _stop = 1;
                    timer1.Enabled = false;
                    timelabel.Text = "Pause";
                    stopbutton.Text = "Continue";
                }
                else
                {
                    _stop = 0;
                    timer1.Enabled = true;
                    stopbutton.Text = "Pause";
                }
            }
        }

```
7. Start game
```
 private void BeginMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap a = new Bitmap("Image\\mouse.png");
            SetCursor(a, new Point(0, 0));
            BuildMap();
            _istart = 1;
            _stop = 0;
            _time = 40;
            _money = 0;
            _point = 0;
            pictureBox1.Invalidate();
            timer3.Enabled = true;
            timer1.Enabled = true;
        }
    
```
### Class Design
* Item: 
```
_Top: Specifies the position to start drawing.
_color: Specifies icon for object .
_size: Specifies the length and width.
_nowpic: Specifies which icon is displayed.
_flag: Whether the flag is selected.
_willclean: Whether to be eliminated.
_PICTURE: Specify the six icons before not being clicked.
_pickpic: Specify the six icons after clicking.
Change(): Change an icon (from the one that has not been clicked to the clicked).
Drawme(): Redraw icons.
```
