﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HslCommunicationDemo
{
    public partial class FormRegister : Form
    {
        public FormRegister( )
        {
            InitializeComponent( );
        }

        private HslCommunication.BasicFramework.SoftAuthorize softAuthorize = null;

        private void FormRegister_Load( object sender, EventArgs e )
        {
            softAuthorize = new HslCommunication.BasicFramework.SoftAuthorize( );            // 实例化
            softAuthorize.ILogNet = new HslCommunication.LogNet.LogNetSingle( "log.txt" );   // 日志
            softAuthorize.FileSavePath = Application.StartupPath + @"\Authorize.txt";        // 设置存储激活码的文件，该存储是加密的
            softAuthorize.LoadByFile( );
        }

        private void button1_Click( object sender, EventArgs e )
        {
            // 获取本机的机器码
            textBox3.Text = softAuthorize.GetMachineCodeString( );
        }

        private void button2_Click( object sender, EventArgs e )
        {
            // 计算该机器码的注册码
            textBox4.AppendText( DateTime.Now.ToString( ) + " [ " + AuthorizeEncrypted( textBox1.Text ) + " ]" + Environment.NewLine );
        }



        /// <summary>
        /// 一个自定义的加密方法，传入一个原始数据，返回一个加密结果
        /// </summary>
        /// <param name="origin"></param>
        /// <returns></returns>
        private string AuthorizeEncrypted( string origin )
        {
            // 此处使用了组件支持的DES对称加密技术
            return HslCommunication.BasicFramework.SoftSecurity.MD5Encrypt( origin, textBox1.Text );
        }

        private void button3_Click( object sender, EventArgs e )
        {
            // 检测激活码是否正确，没有文件，或激活码错误都算作激活失败
            if (!softAuthorize.IsAuthorizeSuccess( AuthorizeEncrypted ))
            {
                // 显示注册窗口
                using (HslCommunication.BasicFramework.FormAuthorize form =
                    new HslCommunication.BasicFramework.FormAuthorize(
                        softAuthorize,
                        "请联系XXX获取激活码",
                        AuthorizeEncrypted ))
                {
                    if (form.ShowDialog( ) != DialogResult.OK)
                    {
                        // 授权失败，退出
                        Close( );
                    }
                    else
                    {
                        // softAuthorize.SaveToFile( );
                    }
                }
            }
            else
            {
                MessageBox.Show( "授权成功！" );
            }
        }

        private void linkLabel1_LinkClicked( object sender, LinkLabelLinkClickedEventArgs e )
        {
            try
            {
                System.Diagnostics.Process.Start( linkLabel1.Text );
            }
            catch(Exception ex)
            {
                HslCommunication.BasicFramework.SoftBasic.ShowExceptionMessage( ex );
            }
        }
    }
}
