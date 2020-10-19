using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace net
{
    public enum itemtype
    {
        无 = 0, cat = 1, monkey = 2, panda = 3, fox = 4, wa = 5,chicken=6
    }

    class Item
    {
        private itemtype _color;//方块颜色
        public itemtype _Color
        {
            get { return _color; }
            set { _color = value; }
        }
        private Point _top;//左上角坐标

        public Point _Top
        {
            get { return _top; }
            set { _top = value; }
        }

        private int _size = 40;//大小

        public int _Size
        {
            get { return _size; }
            set { _size = value; }
        }
         
       // private Bitmap[] picture=new Bitmap[6];
        private Bitmap _nowpic = new Bitmap(40, 40);//现在的图片

        public Bitmap _Nowpic
        {
            get { return _nowpic; }
            set { _nowpic = value; }
        }

        private int _flag;//是否被选中

        public int _Flag
        {
            get { return _flag; }
            set { _flag = value; }
        }


        private int _willclean;//是否将消除

        public int _Willclean
        {
            get { return _willclean; }
            set { _willclean = value; }
        }
        private Bitmap[] _picture = new Bitmap[7];//储存位图
        private Bitmap[] _pickpic = new Bitmap[7];

        //构造函数
        public Item(itemtype color, Point top, int size,int flag)
        {
            _color = color;
            _top = top;
            _size = size;
            _flag = flag;
            for(int i=0;i<=6;i++)
             {
                  //Console.WriteLine("Image\\" + ((itemtype)i).ToString() + ".jpg");
                  _picture[i] = new Bitmap("Image\\" + ((itemtype)i).ToString() + ".jpg");
                  //_picture[i] = new Bitmap(Resource1.((itemtype)i).toString()+".jpg");
             }
            //_picture[0] = new Bitmap(Resource1.cat);
            //
           for (int i = 1; i <= 6; i++)
            {
               _pickpic[i] = new Bitmap("Image\\" + ((itemtype)i).ToString() +"2.jpg");
           }
           _nowpic = _picture[(int)_color];
        }



        public void Drawme(Graphics g)//绘制方块
        {

            if (_flag == 0)//如果被选中改变图片
                {
                    _nowpic = _picture[(int)_color];
                    //_nowpic.MakeTransparent(Color.White);
                }
                g.DrawImage(_nowpic, _top);
            
        }

        public void change()//改变图片样子
        {
            if (_color != itemtype.无)
            {
                _nowpic = _pickpic[(int)_color];
                //_nowpic.MakeTransparent(Color.White);
                _flag = 1;
            }
        }


    }
}
