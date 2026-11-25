using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;

namespace LOPINK_CULL;
static class Program { [STAThread] static void Main(){ ApplicationConfiguration.Initialize(); Application.Run(new MainForm()); } }

class MainForm : Form {
 List<string> files=new(); int index=0; int rating=0; string status="未评分"; PictureBox preview=new(); Label info=new(); FlowLayoutPanel strip=new();
 Dictionary<string,int> ratings=new();
 public MainForm(){
 Text="LOPINK CULL"; Width=1200;Height=800;MinimumSize=new Size(900,600); BackColor=Color.FromArgb(18,18,20);ForeColor=Color.White;
 var top=new Panel{Dock=DockStyle.Top,Height=58,BackColor=Color.FromArgb(30,30,34)};
 var title=new Label{Text="LOPINK CULL",Font=new Font("Segoe UI",16,FontStyle.Bold),ForeColor=Color.White,AutoSize=true,Location=new Point(18,14)};
 var open=new Button{Text="打开文件夹",Location=new Point(200,12),Width=110};open.Click+=(_,_)=>OpenFolder();
 var export=new Button{Text="导出精选",Location=new Point(320,12),Width=110};export.Click+=(_,_)=>Export();
 top.Controls.AddRange(new Control[]{title,open,export});Controls.Add(top);
 preview.Dock=DockStyle.Fill;preview.SizeMode=PictureBoxSizeMode.Zoom;preview.BackColor=Color.Black;Controls.Add(preview);
 var bottom=new Panel{Dock=DockStyle.Bottom,Height=115,BackColor=Color.FromArgb(30,30,34)};info.Dock=DockStyle.Top;info.Height=28;info.TextAlign=ContentAlignment.MiddleCenter;
 strip.Dock=DockStyle.Bottom;strip.Height=80;bottom.Controls.Add(strip);bottom.Controls.Add(info);Controls.Add(bottom);
 var buttons=new FlowLayoutPanel{Dock=DockStyle.Bottom,Height=45,BackColor=Color.FromArgb(24,24,26)};
 foreach(var n in new[]{"1","2","3","4","5","待定","淘汰","上一张","下一张"}){var b=new Button{Text=n,Width=82,Height=35};b.Click+=(_,_)=>{if(n=="上一张")Move(-1);else if(n=="下一张")Move(1);else if(n=="待定")SetRating(-1);else if(n=="淘汰")SetRating(-2);else SetRating(int.Parse(n));};buttons.Controls.Add(b);}
 bottom.Controls.Add(buttons);buttons.BringToFront();
 KeyPreview=true;KeyDown+=(_,e)=>{if(e.KeyCode==Keys.Right)Move(1);else if(e.KeyCode==Keys.Left)Move(-1);else if(e.KeyCode>=Keys.D1&&e.KeyCode<=Keys.D5)SetRating(e.KeyCode-Keys.D0);else if(e.KeyCode==Keys.Delete)SetRating(-2);};
 }
 void OpenFolder(){using var d=new FolderBrowserDialog();if(d.ShowDialog()!=DialogResult.OK)return;files=Directory.GetFiles(d.SelectedPath).Where(f=>new[]{".jpg",".jpeg",".png",".bmp",".gif",".tif",".tiff"}.Contains(Path.GetExtension(f).ToLower())).OrderBy(f=>f).ToList();index=0;ratings.Clear();ShowImage();}
 void ShowImage(){if(files.Count==0){preview.Image=null;info.Text="打开文件夹以开始";return;}try{using var s=File.OpenRead(files[index]);using var img=Image.FromStream(s);preview.Image=new Bitmap(img);}catch{preview.Image=null;}rating=ratings.GetValueOrDefault(files[index]);status=rating>0?$"{rating} 星":rating==-1?"待定":rating==-2?"淘汰":"未评分";info.Text=$"{index+1}/{files.Count}  |  {Path.GetFileName(files[index])}  |  {status}";strip.Controls.Clear();foreach(var f in files.Take(300)){var l=new Label{Text=Path.GetFileName(f),Width=120,Height=24,ForeColor=ratings.GetValueOrDefault(f)>0?Color.Gold:Color.LightGray};strip.Controls.Add(l);}}
 void Move(int d){if(files.Count==0)return;index=Math.Clamp(index+d,0,files.Count-1);ShowImage();}
 void SetRating(int r){if(files.Count==0)return;ratings[files[index]]=r;Move(0);}
 void Export(){if(files.Count==0)return;using var d=new FolderBrowserDialog{Description="选择精选照片导出目录"};if(d.ShowDialog()!=DialogResult.OK)return;var selected=files.Where(f=>ratings.GetValueOrDefault(f)>0).ToList();foreach(var f in selected){var dest=Path.Combine(d.SelectedPath,Path.GetFileName(f));if(!File.Exists(dest))File.Copy(f,dest);}MessageBox.Show($"已复制 {selected.Count} 张精选照片。","导出完成");}
}