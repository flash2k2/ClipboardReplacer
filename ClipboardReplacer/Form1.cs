using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ClipboardReplacer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var dataobject = Clipboard.GetDataObject();
            var formats = dataobject.GetFormats();
            if (formats.Length == 0)
            {
                toolStripStatusLabel1.Text = "Empty clipboard";
                return;
            };
            var data = dataobject.GetData(formats[0]);
            if (data is MemoryStream)
            {
                var array = (data as MemoryStream).ToArray();
                var source = Encoding.Unicode.GetBytes(tbSource.Text);
                var dest = Encoding.Unicode.GetBytes(tbDest.Text);
                int count = 0;
                var newArray = Replace(array, source, dest, out count);
                Clipboard.SetData(formats[0], new MemoryStream(newArray));
                toolStripStatusLabel1.Text = $"Replaced {count} times";
            }
            else if (data is byte[])
            {
                var array = (data as byte[]);
                var source = Encoding.Unicode.GetBytes(tbSource.Text);
                var dest = Encoding.Unicode.GetBytes(tbDest.Text);
                int count = 0;
                var newArray = Replace(array, source, dest, out count);
                Clipboard.SetData(formats[0], newArray);
                toolStripStatusLabel1.Text = $"Replaced {count} times";
            }
            else if (data is String)
            {
                var newString = (data as String).Replace(tbSource.Text, tbDest.Text);
                Clipboard.SetData(formats[0], newString);
                toolStripStatusLabel1.Text = $"Replaced";
            }
            else
            {
                toolStripStatusLabel1.Text = "Unsupported format";
            };
        }

        private static byte[] Replace(byte[] input, byte[] pattern, byte[] replacement, out int count)
        {
            count = 0;
            if (pattern.Length == 0)
            {
                return input;
            }

            List<byte> result = new List<byte>();
            int i;
          
            for (i = 0; i <= input.Length - pattern.Length; i++)
            {
                bool foundMatch = true;
                for (int j = 0; j < pattern.Length; j++)
                {
                    if (input[i + j] != pattern[j])
                    {
                        foundMatch = false;
                        break;
                    }
                }

                if (foundMatch)
                {
                    result.AddRange(replacement);
                    i += pattern.Length - 1;
                    count++;
                }
                else
                {
                    result.Add(input[i]);
                }
            }

            for (; i < input.Length; i++)
            {
                result.Add(input[i]);
            }

            return result.ToArray();
        }

        private void btnMove_Click(object sender, EventArgs e)
        {
            tbSource.Text = tbDest.Text;
            tbDest.Text = "";
        }
    }

}
