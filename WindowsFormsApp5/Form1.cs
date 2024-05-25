using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Collections;

namespace WindowsFormsApp5
{
    public partial class Form1 : Form
    {
        private static String VertSourceFile="", VertDestFile="", VishenerSourceFile="", VishenerDestFile="";

        private void VertReadSource(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            String VertSourceFile = openFileDialog1.FileName;
            button1.Enabled = false;
        }

        public Form1()
        {
            InitializeComponent();
            
        }

        private string getkey(string key) {
            key=key.ToLower();
            key = key.Trim();
            StringBuilder temp = new StringBuilder();
            for(int i = 0; i < key.Length; i++) { 
                if((key[i]>='a' && key[i] <= 'z') || (key[i]=='_')) { temp.Append(key[i]);}
            }
            return temp.ToString();
        }

        private void VishnenerReadSource(object sender, EventArgs e)
        {
            openFileDialog3.ShowDialog();
            String VertSourceFile = openFileDialog1.FileName;
            button2.Enabled = false;
        }

        private void VertDest(object sender, EventArgs e)
        {
            openFileDialog2.ShowDialog();
            String VertSourceFile = openFileDialog2.FileName;
            button8.Enabled = false;
        }

        private int[] getnum(string arr) {
            int[] res = new int[arr.Length];
            char start = 'a';
            char end = 'z';
            int ind = 1;
            while (start <= end) {
                for (int i = 0; i < arr.Length; i++) {
                    if (arr[i] == start) {
                        res[i] = ind;
                        ind++;
                    }
                }
                start++;
            }
            return res;
        }

        private int[] getind(int[] arr) {
            int[] res = new int[arr.Length];
            int j = 0,ind=1;
            while (ind <= arr.Length)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    if (arr[i] == ind)
                    {
                        res[j] = i;
                        j++;
                        ind++;
                        break;
                    }
                }
            }
            return res;
        }

        private void VishenerDest(object sender, EventArgs e)
        {
            openFileDialog4.ShowDialog();
            String VertSourceFile = openFileDialog1.FileName;
            button7.Enabled = false;
        }

        private char[,] createtable() {
            char[,] table = new char[33, 33];
            string alphabet ="АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
            alphabet = alphabet.ToLower();
            int startind = 0;
            for (int i = 0; i < 33; i++) {
                int ind = startind;
                for (int j = 0; j < 33; j++) {
                    table[i, j] = alphabet[ind];
                    ind++;
                    if (ind == 33) ind = 0;
                }
                startind++;
            }
            return table;
        }

        private int gettableind(char c) {
            Dictionary<char, int> map = new Dictionary<char, int>() {
                {'а',0},{'б',1},{'в',2},{'г',3},{'д',4},{'е',5},{'ё',6},{'ж',7},{'з',8},{'и',9},{'й',10},{'к',11},{'л',12},{'м',13},{'н',14},{'о',15},{'п',16},{'р',17},{'с',18},{'т',19},{'у',20},{'Ф',21},{'х',22},{'ц',23},{'ч',24},{'ш',25},{'щ',26},{'ъ',27},{'ы',28},{'ь',29},{'э',30},{'ю',31},{'я',32}
            };
            return map[c];
        }
        private string ruskey(string key)
        {
            key = key.ToLower();
            key = key.Trim();
            StringBuilder temp = new StringBuilder();
            for (int i = 0; i < key.Length; i++)
            {
                if ((key[i] >= 'а' && key[i] <= 'я') || (key[i] == 'ё')) { temp.Append(key[i]); }
            }
            return temp.ToString();
        }

        private void VishenerCipher(object sender, EventArgs e)
        {
            VishenerSourceFile = openFileDialog3.FileName;
            VishenerDestFile = openFileDialog4.FileName;
            if (VishenerDestFile != "" && VishenerSourceFile != "")
            {
                char[,] table = createtable();
                string key = ruskey(textBox2.Text);
                StringBuilder plain = new StringBuilder();
                int keyind = 0, keyaddind = 0;
                StringBuilder ciphertext = new StringBuilder();
                StringBuilder keytext = new StringBuilder(key);
                StreamReader reader = new StreamReader(VishenerSourceFile);
                StringBuilder text = new StringBuilder();
                while (!reader.EndOfStream)
                {
                    char temp;
                    temp = (char)reader.Read();
                    if ((temp >= 'а' && temp <= 'я') || temp == 'ё') text.Append(temp);
                }
                string plaint = text.ToString();
                int ind = 0;
                while (ind < plaint.Length)
                {
                    char c = plaint[ind], plainc;
                    ind++;
                    if ((c >= 'а' && c <= 'я') || c == 'ё')
                    {
                        plain.Append(c);
                        if (keyind < key.Length)
                        {
                            int i, j;
                            i = gettableind(key[keyind]);
                            j = gettableind(c);
                            plainc = table[i, j];
                            keyind++;
                        }
                        else
                        {
                            keytext.Append(plain.ToString()[keyaddind]);
                            keyaddind++;
                            key = keytext.ToString();
                            int i, j;
                            i = gettableind(key[keyind]);
                            j = gettableind(c);
                            plainc = table[i, j];
                            keyind++;
                        }
                        ciphertext.Append(plainc);
                    }
                }
                StreamWriter writer = new StreamWriter(VishenerDestFile, false);
                writer.Write(ciphertext.ToString());
                button2.Enabled = true;
                button7.Enabled = true;
                textBox2.Text = "";
                reader.Close();
                writer.Flush();
                writer.Close();
                openFileDialog3.FileName = "";
                openFileDialog4.FileName = "";
            }
            else
            {
                Form2 exc = new Form2();
                exc.Show();
            }
        }

        private void VishenerDecipher(object sender, EventArgs e)
        {
            VishenerSourceFile = openFileDialog3.FileName;
            VishenerDestFile = openFileDialog4.FileName;
            if (VishenerDestFile != "" && VishenerSourceFile != "")
            {
                char[,] table = createtable();
                string key = ruskey(textBox2.Text);
                StringBuilder plain = new StringBuilder();
                int keyind = 0, keyaddind = 0;
                StringBuilder keytext = new StringBuilder(key);
                StreamReader reader = new StreamReader(VishenerSourceFile);
                StringBuilder text = new StringBuilder();
                while (!reader.EndOfStream)
                {
                    char temp;
                    temp = (char)reader.Read();
                    if ((temp >= 'а' && temp <= 'я') || temp == 'ё') text.Append(temp);
                }
                string ciphertext = text.ToString();
                ciphertext = ciphertext.Trim();
                int m = 0;
                while (m<ciphertext.Length)
                {
                    if ((ciphertext[m] >= 'а' && ciphertext[m] <= 'я') || ciphertext[m] == 'ё')
                    {
                        if (key != "")
                        {
                            if (keyind < key.Length)
                            {
                                int i, j = 0;
                                i = gettableind(key[keyind]);
                                for (int l = 0; l < 33; l++)
                                {
                                    if (table[i, l] == ciphertext[m]) { j = l; break; }
                                }
                                plain.Append(table[0, j]);
                                m++;
                                keyind++;
                            }
                            else
                            {
                                keytext.Append(plain.ToString()[keyaddind]);
                                keyaddind++;
                                key = keytext.ToString();
                                int i, j = 0;
                                i = gettableind(key[keyind]);
                                i = gettableind(key[keyind]);
                                for (int l = 0; l < 33; l++)
                                {
                                    if (table[i, l] == ciphertext[m]) { j = l; break; }
                                }
                                plain.Append(table[0, j]);
                                m++;
                                keyind++;
                            }
                        }
                        else
                        {
                            int i = 0;
                            for (int l = 0; l < 33; l++)
                            {
                                if (table[l, l] == ciphertext[m]) { i = l; break; }
                            }
                            plain.Append(table[0, i]);
                            m++;
                        }
                    }
                }
                string res = plain.ToString();
                StreamWriter writer = new StreamWriter(VishenerDestFile, false);
                writer.Write(res);
                button2.Enabled = true;
                button7.Enabled = true;
                openFileDialog3.FileName = "";
                openFileDialog4.FileName = "";  
                textBox2.Text = "";
                reader.Close();
                writer.Flush();
                writer.Close();
            }
            else
            {
                Form2 exc = new Form2();
                exc.Show();
            }
        }
        

        private int[] calcheight(String str,int keylen,int[] ind) {
            int[] height = new int[2];
            height[0] = 1;
            height[1] = 0;
            int len = str.Length;
            for (int i = 1; i <= keylen; i++)
            {
                len -= i;
            }
            if (len>0)
            {
                height[0] = keylen;
                if (len % keylen != 0)
                {
                    height[1] = len % keylen;
                    height[0] += ((len - len % keylen) / keylen);
                    height[0]++;
                }
                else height[0] += (len / keylen);
                return height;
            }
            else if (len==0)
            {
                return new int[] { keylen, 0};
            }
            else if (len<0) {
                len = str.Length;                
                for (int i = 0; i < keylen; i++) {
                    len -= (ind[i]+1);
                    if (len > 0) height[0]++; else return height;
                }
            }
            return height;
        }

        private void DecipherVert(object sender, EventArgs e)
        {
            VertSourceFile = openFileDialog1.FileName;
            VertDestFile = openFileDialog2.FileName;
            String key = getkey(textBox1.Text);
            if (VertSourceFile != "" && VertDestFile != "" && key != "")
            {
                FileStream file = new FileStream(VertSourceFile, FileMode.Open, FileAccess.Read);
                StreamReader reader = new StreamReader(VertSourceFile);
                List<char[]> list = new List<char[]>();
                int[] num = getnum(key);
                int j = 0;
                int[] indexes = getind(num);
                StringBuilder text = new StringBuilder();
                while (!reader.EndOfStream)
                {
                    char temp;
                    temp = (char)reader.Read();
                    if (temp >= 'a' && temp <= 'z') text.Append(temp);
                }
                string ciphertext = text.ToString();
                ciphertext = ciphertext.Trim();
                ciphertext = getkey(ciphertext);
                int[] arr = calcheight(ciphertext, key.Length, indexes);
                int h = arr[0];
                bool[,] matrix = new bool[h, key.Length];
                int row = 0;
                for (int i = 0; i < key.Length; i++)
                {
                    char[] temp = new char[key.Length];
                    while (num[j] != i + 1)
                    {
                        if (row < h) matrix[row, j] = true; else break;
                        j++;
                    }
                    if (row < h) matrix[row, j] = true; else break;
                    j = 0;
                    row++;
                }
                j = 0;
                while (row < h - 1)
                {
                    matrix[row, j] = true;
                    if (j == key.Length - 1)
                    {
                        j = 0;
                        row++;
                    }
                    else j++;
                }
                if (arr[1] != 0)
                {
                    for (int i = 0; i < arr[1]; i++) { if (row < h) matrix[row, i] = true; else break; }
                }
                else for (int i = 0; i < key.Length; i++) { if (row < h) matrix[row, i] = true; else break; }
                char[,] message = new char[h, key.Length];
                j = 0;
                for (int i = 0; i < key.Length; i++)
                {
                    int n = indexes[i];
                    for (int l = 0; l < h; l++)
                    {
                        if (matrix[l, n])
                        {
                            message[l, n] = ciphertext[j];
                            j++;
                        }
                    }
                }
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < h; i++)
                {
                    for (int l = 0; l < key.Length; l++)
                    {
                        if (message[i, l] != '\0') stringBuilder.Append(message[i, l]);
                    }
                }
                StreamWriter writer = new StreamWriter(VertDestFile, false);
                String res = stringBuilder.ToString();
                writer.Write(res);
                button1.Enabled = true;
                button8.Enabled = true;
                openFileDialog1.FileName = "";
                openFileDialog2.FileName = "";
                textBox1.Text = "";
                reader.Close();
                writer.Flush();
                writer.Close();
            }
            else {
                Form2 exc = new Form2();
                exc.Show();
            }
        }

        private void CipherVert(object sender, EventArgs e)
        {
            VertSourceFile = openFileDialog1.FileName;
            VertDestFile = openFileDialog2.FileName;
            String key = getkey(textBox1.Text);
            if (VertSourceFile != "" && VertDestFile!="" && key!="")
            {
                FileStream file = new FileStream(VertSourceFile, FileMode.Open, FileAccess.Read);
                StreamReader reader = new StreamReader(VertSourceFile);
                StringBuilder text = new StringBuilder();
                while (!reader.EndOfStream) {
                    char temp;
                    temp = (char)reader.Read();
                    if (temp >= 'a' && temp <= 'z') text.Append(temp);
                }
                string plain = text.ToString();
                int ind = 0;
                List<char[]> list = new List<char[]>();
                int[] num = getnum(key);
                int j = 0;
                for (int i = 0; i < key.Length; i++) {
                    char[] temp = new char[key.Length];
                    while (num[j] != i + 1) {
                        if (ind < plain.Length) {
                            char c = plain[ind];
                            if ((c >= 'a' && c <= 'z') || (c =='_')) temp[j] = c;
                            ind++;
                        } 
                        j++;
                    }
                    if (ind < plain.Length)
                    {
                        char c = plain[ind];
                        if ((c >= 'a' && c <= 'z') || (c == '_')) temp[j] = c;
                        ind++;
                    }
                    j = 0;
                    list.Add(temp);
                }
                j = 0;
                char[] temp1 = new char[key.Length];
                while (ind<plain.Length)
                {
                    if (ind < plain.Length)
                    {
                        char c = plain[ind];
                        if ((c >= 'a' && c <= 'z') || (c == '_')) temp1[j] = c;
                        ind++;
                    }
                    if (j == key.Length - 1)
                    {
                        j = 0;
                        list.Add(temp1);
                        temp1 = new char[key.Length];
                    }
                    else j++;
                }
                list.Add(temp1);
                int[] indexes = getind(num);
                char[] elem = new char[key.Length];
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < key.Length; i++) {
                    int n = indexes[i];
                    for (int l = 0; l < list.Count; l++) {
                        elem = list.ElementAt(l);
                        if(elem[n]!='\0') stringBuilder.Append(elem[n]);
                    }
                }
                StreamWriter writer = new StreamWriter(VertDestFile,false);
                String res = stringBuilder.ToString();
                writer.Write(res);
                button1.Enabled = true;
                button8.Enabled = true;
                openFileDialog1.FileName = "";
                openFileDialog2.FileName = "";
                textBox1.Text = "";
                reader.Close();
                writer.Flush();
                writer.Close();
            }
            else
            {
                Form2 exc = new Form2();
                exc.Show();
            }


        }
    }
}
