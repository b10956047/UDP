using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _220328
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        UdpClient U;
        Thread Th;

        //監聽副程式
        private void Listen()
        {
            //設定監聽用的通訊埠
            int Port = int.Parse(textBox_listenPort.Text);

            //監聽UDP監聽器實體
            U = new UdpClient(Port);

            //建立本機端點資訊
            IPEndPoint EP =  new IPEndPoint(IPAddress.Parse("127.0.0.1"), Port);

            while (true)
            {
                byte[] B = U.Receive(ref EP);//接收到的訊息放到B陣列
                textBox_receiveMsg.Text = Encoding.Default.GetString(B);//翻譯B陣列為字串
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;//忽略跨執行續錯誤
            Th = new Thread(Listen);//建立監聽執行緒，目標副程式 -> Listen
            Th.Start();//啟動監聽執行緒
            button_startListen.Enabled = false;//使button失效(不能重複開啟監聽)
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                Th.Abort();//關閉監聽執行緒
                U.Close();//關閉監聽器
            }
            catch
            {
                //忽略錯誤
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string IP = textBox_targetIP.Text;//設定發送目標IP
            int Port = int.Parse(textBox_listenPort.Text);//設定發送目標Port
            byte[] B = Encoding.Default.GetBytes(textBox_sendMsg.Text);//字串翻譯成位元
            UdpClient S = new UdpClient();//建立UDP Client
            S.Send(B, B.Length, IP, Port);//發送資料到指定位置
            S.Close();///關閉UDP Client
        }
    }
}
