using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _023.简单播放器
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            musicPlayer.Ctlcontrols.play();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            musicPlayer.Ctlcontrols.pause();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            musicPlayer.Ctlcontrols.stop();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //程序加载时，取消自动播放；
            musicPlayer.settings.autoStart = false;

            //musicPlayer.URL = @"D:\学习材料\C语言\我的练习\C#练习\01. C#入门到精通\十五、基础加强总复习\023. 简单播放器\music\周杰伦 - 算什么男人.wav";
        }

        bool b = true;

        /// <summary>
        /// 播放或者暂停按钮效果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPlayOrPause_Click(object sender, EventArgs e)
        {
            //当音乐列表为空时，点击播放按钮提示：
            if (listBox1.Items.Count == 0)
            {
                MessageBox.Show("Please add music first!");
                return;
            }


            if (btnPlayOrPause.Text == "Play")
            {

                if (b)
                {//获得选中的歌曲；让音乐从头播放；
                    try
                    {
                        musicPlayer.URL = listPath[listBox1.SelectedIndex];

                        IsExistLrc(listPath[listBox1.SelectedIndex]);

                    }
                    catch
                    {

                    }
                }

                musicPlayer.Ctlcontrols.play();

                btnPlayOrPause.Text = "Pause";
            }
            else
            {
                musicPlayer.Ctlcontrols.pause();

                btnPlayOrPause.Text = "Play";

                b = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            musicPlayer.Ctlcontrols.stop();

            btnPlayOrPause.Text = "Play";
        }



        //存储音乐文件的全路径；
        List<string> listPath = new List<string>();

        /// <summary>
        /// 打开对话框，选择音乐
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Please choose media";
            ofd.Filter = "video|*.mp4;*.wma;*avi|music|*.mp3;*.flac;*.ape;*.wav|all|*.*";
            ofd.InitialDirectory = Application.StartupPath + @"\music";
            ofd.Multiselect = true;
            ofd.ShowDialog();

            string[] path = ofd.FileNames;

            for (int i = 0; i < path.Length; i++)
            {
                //将音乐文件的全路径存储到list集合中；
                listPath.Add(path[i]);

                //将音乐文件的歌名，放进listBox中；
                listBox1.Items.Add(Path.GetFileNameWithoutExtension(path[i]));
            }
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (listBox1.Items.Count == 0)
            {
                MessageBox.Show("There is no music yet!");
            }
            else
            {
                if (listBox1.SelectedIndex == -1)
                {
                    MessageBox.Show("Please click music");
                }
                else
                {
                    musicPlayer.URL = listPath[listBox1.SelectedIndex];
                    musicPlayer.Ctlcontrols.play();
                    btnPlayOrPause.Text = "Pause";

                    lblInformation.Text = musicPlayer.currentMedia.duration.ToString();

                    //加载歌词函数；
                    IsExistLrc(listPath[listBox1.SelectedIndex]);
                }
            }

        }


        /// <summary>
        /// 上一曲
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            //切换音乐后，清空两个集合中的内容；
            listPlayTime.Clear();
            listLrc.Clear();

            //当音乐列表为空时，提示用户添加音乐；
            if (listBox1.Items.Count == 0)
            {
                MessageBox.Show("You have not added any music yet!");
                return;
            }

            //获得当前选中项的索引；
            int index = listBox1.SelectedIndex;

            //清空当前listBox1索引；
            listBox1.SelectedIndex = -1;

            index--;

            if (index < 0)
            {
                index = listPath.Count - 1;
            }

            //将改变后的索引，重新赋值给listBox1的索引；
            listBox1.SelectedIndex = index;

            //listBox1.SelectedItem = listBox1[i];

            //加载歌词；
            IsExistLrc(listPath[listBox1.SelectedIndex]);

            musicPlayer.URL = listPath[index];

            musicPlayer.Ctlcontrols.play();

            btnPlayOrPause.Text = "Pause";
        }


        /// <summary>
        /// 下一曲
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button7_Click(object sender, EventArgs e)
        {
            //点击时，清空两个泛型集合的内容；
            listPlayTime.Clear();
            listLrc.Clear();


            //当音乐列表为空时，提示用户添加音乐；
            if (listBox1.Items.Count == 0)
            {
                MessageBox.Show("You have not added any music yet!");
                return;
            }


            //获得当前选中项的索引；
            int index = listBox1.SelectedIndex;

            //清空当前listBox1索引；
            listBox1.SelectedIndex = -1;

            index++;

            if (index > listPath.Count - 1)
            {
                index = 0;
            }

            //将改变后的索引，重新赋值给listBox1的索引；
            listBox1.SelectedIndex = index;

            //加载歌词；
            IsExistLrc(listPath[listBox1.SelectedIndex]);


            //listBox1.SelectedItem = listBox1[i];

            musicPlayer.URL = listPath[index];

            musicPlayer.Ctlcontrols.play();

            btnPlayOrPause.Text = "Pause";
        }


        /// <summary>
        /// 点击删除选中项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //删除列表中的选中项；
            //先从集合中删除；
            //获取要删除歌曲的数量；
            int count = listBox1.SelectedItems.Count;

            for (int i = 1; i <= count; i++)
            {
                //先从集合中删除；
                listPath.RemoveAt(listBox1.SelectedIndex);
                //再从列表中删除；
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            }






        }


        /// <summary>
        /// 点击放音或静音
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void label1_Click(object sender, EventArgs e)
        {
            if (label1.Tag.ToString() == "1")
            {
                //1表示放音，目的是让你静音；
                musicPlayer.settings.mute = true;

                label1.Tag = "2";

                label1.Image = Image.FromFile(@"..\..\images\silence.png");

            }
            else if (label1.Tag.ToString() == "2")
            {
                //当tag=2时，表示当前时静音，我要让你放音；
                musicPlayer.settings.mute = false;

                label1.Tag = "1";

                label1.Image = Image.FromFile(@"..\..\images\play.png");

            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnPlayOrPause.Text = "Play";

            b = true;
        }

        /// <summary>
        /// 增加音量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button8_Click(object sender, EventArgs e)
        {
            musicPlayer.settings.volume += 5;
        }

        /// <summary>
        /// 减少音量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button9_Click(object sender, EventArgs e)
        {
            musicPlayer.settings.volume -= 5;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //如果播放器的状态为正在播放中，则计时；
            if (musicPlayer.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                lblInformation.Text = musicPlayer.currentMedia.duration.ToString() + "\r\n" + musicPlayer.currentMedia.durationString + "\r\n" + musicPlayer.Ctlcontrols.currentPosition.ToString() + "\r\n" + musicPlayer.Ctlcontrols.currentPositionString;

                #region 自动播放下一曲方法1：
                ////获取歌曲总时间和当前歌曲播放的时间；
                //double totalTime = musicPlayer.currentMedia.duration;
                //double currentTime = musicPlayer.Ctlcontrols.currentPosition;

                ////如果歌曲播放的当前时间大于歌曲的总时间，则自动播放下一曲；
                //if (currentTime + 1 > totalTime)
                //{
                //    //获得当前选中项的索引；
                //    int index = listBox1.SelectedIndex;

                //    //清空当前listBox1索引；
                //    listBox1.SelectedIndex = -1;

                //    index++;

                //    if (index > listPath.Count - 1)
                //    {
                //        index = 0;
                //    }

                //    //将改变后的索引，重新赋值给listBox1的索引；
                //    listBox1.SelectedIndex = index;

                //    //listBox1.SelectedItem = listBox1[i];

                //    musicPlayer.URL = listPath[index];

                //    musicPlayer.Ctlcontrols.play();

                //    btnPlayOrPause.Text = "暂停";
                //}
                #endregion


            }


        }

        /// <summary>
        /// 自动播放下一曲方法2：当播放状态发生改变时，触发本事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void musicPlayer_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            //如果当前播放器的状态是MediaEnded（播放结束），自动播放下一曲；
            if (musicPlayer.playState == WMPLib.WMPPlayState.wmppsMediaEnded)
            {
                int index = listBox1.SelectedIndex;

                listBox1.SelectedIndex = -1;

                index++;

                if (index > listBox1.Items.Count - 1)
                {
                    index = 0;
                }

                listBox1.SelectedIndex = index;

                musicPlayer.URL = listPath[index];

            }

            if (musicPlayer.playState == WMPLib.WMPPlayState.wmppsReady)
            {
                try
                {
                    musicPlayer.Ctlcontrols.play();
                }
                catch
                {

                }
            }
        }



        /// <summary>
        /// 歌词加载功能实现
        /// </summary>
        void IsExistLrc(string songPath)
        {
            //加载歌词前，应该先清空两个集合的内容；
            lblLyc.Text = "";
            listLrc.Clear();
            listPlayTime.Clear();

            string lrc = Path.GetFileNameWithoutExtension(songPath) + ".lrc";
            string lrcPath = Path.GetDirectoryName(songPath) +"\\"+ lrc;

            //MessageBox.Show(lrcPath);

            if (File.Exists(lrcPath))//如果当前歌曲有歌词；则读取歌词文件；
            {
                //MessageBox.Show("ok！");
                string[] lycLines = File.ReadAllLines(lrcPath, Encoding.Default);

                //格式化歌词方法；
                FormatLrc(lycLines);

            }
            else//如果当前歌曲没歌词
            {
                lblLyc.Text = "---No lyrics found---";
            }
        }

        //用于存储时间；
        List<double> listPlayTime = new List<double>();

        //用于存储歌词；
        List<string> listLrc = new List<string>();


        /// <summary>
        /// 格式化歌词；
        /// </summary>
        /// <param name="lrcLines"></param>
        void FormatLrc(string[] lrcLines)
        {
            for (int i = 0; i < lrcLines.Length; i++)
            {
                try
                {
                    string[] lrcTemp = lrcLines[i].Split(new char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);

                    string[] lrcNewTemp = lrcTemp[0].Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

                    double playTime = double.Parse(lrcNewTemp[0]) * 60 + double.Parse(lrcNewTemp[1]);

                    //将截取出来的时间加到泛型集合中；
                    listPlayTime.Add(playTime);

                    //将这个时间对应的歌词存储到泛型集合中；
                    listLrc.Add(lrcTemp[1]);
                }
                catch
                {

                }

                

            }
        }

        /// <summary>
        /// 每秒判断一次，播放歌词
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer2_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < listPlayTime.Count; i++)
            {
                if (musicPlayer.Ctlcontrols.currentPosition >= listPlayTime[i] && musicPlayer.Ctlcontrols.currentPosition < listPlayTime[i + 1])
                {
                    lblLyc.Text = listLrc[i];

                    label1.Text = listPlayTime[i].ToString();

                    
                }

            }

        }
    }
}
