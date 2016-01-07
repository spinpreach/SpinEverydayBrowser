using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;

namespace Spinpreach.MonsterGirlsPlayer
{
    public class MonsterGirlsBrowser : WebBrowser
    {
        const string GAME_URL = "http://www.dmm.com/netgame/social/-/gadgets/=/app_id=365723/";

        public Action LoginCompletedEvent;
        public Action<Exception> LoginErrorEvent;
        public Action<bool> MuteChangedEvent;
        public Action<float> ZoomChangedEvent;

        private LoginInfo login { get; set; }
        private Audio audio;

        public MonsterGirlsBrowser()
        {
            RegistryHelper.SetBrowserEmulation(RegistryHelper.BROWSER_VERSION.IE9);
            this.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(this.MonsterGirlsBrowser_DocumentCompleted);
            this.Resize += new EventHandler(this.MonsterGirlsBrowser_Zoom);
        }

        private void MonsterGirlsBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                var Browser = (WebBrowser)sender;
                if (e.Url.PathAndQuery.StartsWith("/my/-/login/=/path="))
                {
                    var inputs = Browser.Document.GetElementsByTagName("input");
                    var LoginId = inputs.Cast<HtmlElement>().Single(el => el.Id == "login_id");
                    var Password = inputs.Cast<HtmlElement>().Single(el => el.Id == "password");
                    var Submit = inputs.Cast<HtmlElement>().Single(x => x.OuterHtml.Contains("submit"));

                    LoginId.SetAttribute("value", this.login.UserID);
                    Password.SetAttribute("value", this.login.PassWord);
                    Submit.InvokeMember("click");
                }
                else if (e.Url.ToString() == GAME_URL)
                {
                    // 少しタイミングを遅らせる。
                    this.CompletedAsync();
                }
            }
            catch (Exception ex)
            {
                this.LoginErrorEvent?.Invoke(ex);
            }
        }

        private async void CompletedAsync()
        {
            await Task.Run(() => { System.Threading.Thread.Sleep(1500); }); // ← 要調整（画面読み込み待機）
            this.documentChanger();
            this.frameReSize(this.Width, this.Height);
            this.audio = new Audio();
            this.audio.MuteChangedEvent += (mute) => this.Invoke(new Action<bool>(this.Audio_MuteChanged), mute);
            this.Resize += new EventHandler(this.MonsterGirlsBrowser_Resize);
            this.LoginCompletedEvent?.Invoke(); // ← 読み込み完了通知は最後！！
        }

        private void Audio_MuteChanged(bool mute)
        {
            if (this.IsMute() == null) return;
            this.MuteChangedEvent?.Invoke(mute);
        }

        private void MonsterGirlsBrowser_Resize(object sender, EventArgs e)
        {
            this.frameReSize(this.Width, this.Height);
        }

        private void MonsterGirlsBrowser_Zoom(object sender, EventArgs e)
        {
            var wz = (double)this.Width / (double)960 * 100;
            var hz = (double)this.Height / (double)540 * 100;
            var zm = wz < hz ? wz : hz;
            zm = zm * 100;
            zm = Math.Floor(zm);
            zm = zm / 100;
            var obj = (float)zm;
            this.ZoomChangedEvent?.Invoke(obj);
        }

        #region method

        private mshtml.HTMLDocument getFrameById(mshtml.HTMLDocument document, string frameId)
        {
            try
            {

                if (document == null) return null;

                var frame = document.getElementById(frameId) as mshtml.HTMLFrameElement;
                if (frame == null) return null;

                var window = frame.contentWindow as mshtml.HTMLWindow2;
                if (window == null) return null;

                var provider = window as IServiceProvider;
                if (provider == null) return null;

                var guidService = typeof(SHDocVw.IWebBrowserApp).GUID;
                var riid = typeof(SHDocVw.IWebBrowser2).GUID;
                object ppvObject;
                provider.QueryService(guidService, riid, out ppvObject);

                var webBrowser = ppvObject as SHDocVw.IWebBrowser2;
                if (webBrowser == null) return null;

                return webBrowser.Document as mshtml.HTMLDocument;

            }
            catch (Exception)
            {
                return null;
            }
        }

        private mshtml.HTMLDivElement getDivElementsByClassName(mshtml.HTMLDocument document, string className)
        {
            try
            {
                if (document == null) return null;
                var divs = document.getElementsByTagName("div");
                foreach (mshtml.HTMLDivElement item in divs)
                {
                    if (item.className == className)
                    {
                        return item;
                    }
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private mshtml.HTMLParaElement getParaElementsByClassName(mshtml.HTMLDocument document, string className)
        {
            try
            {
                if (document == null) return null;
                var paras = document.getElementsByTagName("p");
                foreach (mshtml.HTMLParaElement item in paras)
                {
                    if (item.className == className)
                    {
                        return item;
                    }
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void documentChanger()
        {
            try
            {

                #region css

                StringBuilder css = new StringBuilder();
                css.Append("body");
                css.Append("{");
                css.Append("    overflow: hidden;");
                css.Append("}");
                css.Append("iframe");
                css.Append("{");
                css.Append("    position: fixed;");
                css.Append("    z-index: 1;");
                css.Append("}");

                #endregion

                var document = this.Document.DomDocument as mshtml.HTMLDocument;
                if (document == null) return;

                document.createStyleSheet().cssText = css.ToString();

                var div01 = document.getElementById("dmm-ntgnavi-renew");
                var div02 = document.getElementById("ntg-recommend");
                var div03 = this.getDivElementsByClassName(document, "area-naviapp mg-t20");
                var div04 = document.getElementById("foot");

                if (div01 != null) div01.style.display = "none";
                if (div02 != null) div02.style.visibility = "hidden";
                if (div03 != null) div03.style.display = "none";
                if (div04 != null) div04.style.display = "none";

                //********************************************************************
                // game_frame
                //********************************************************************
                var document2 = this.getFrameById(document, "game_frame");

                var div11 = document2.getElementById("spacing_top");
                //var div12 = document2.getElementById("flashWrap");
                var div13 = this.getParaElementsByClassName(document2, "ms_bnrArea");
                var div14 = document2.getElementById("area-annex");

                if (div11 != null) div11.style.display = "none";
                if (div13 != null) div13.style.display = "none";
                if (div14 != null) div14.style.display = "none";

            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Exception : {0}.{1} >> {2}", ex.TargetSite.ReflectedType.FullName, ex.TargetSite.Name, ex.Message));
            }
        }

        private void frameReSize(int width, int height)
        {
            try
            {

                if (this.Document == null) return;

                var document = this.Document.DomDocument as mshtml.HTMLDocument;
                if (document == null) return;

                var frame = document.getElementById("game_frame");
                if (frame == null) return;

                var swf = this.getFrameById(document, "game_frame")?.getElementById("externalswf") as mshtml.HTMLEmbed;
                if (swf == null) return;

                frame.style.width = string.Format("{0}px", width);
                if (height < 540)
                {
                    var offset = (540 - height) / 2;
                    frame.style.top = string.Format("-{0}px", offset);
                    frame.style.height = string.Format("{0}px", 540 - offset);
                }
                else
                {
                    frame.style.top = string.Format("{0}px", 0);
                    frame.style.height = string.Format("{0}px", height);
                }

                swf.style.width = string.Format("{0}px", width);
                swf.style.height = string.Format("{0}px", height);

            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Exception : {0}.{1} >> {2}", ex.TargetSite.ReflectedType.FullName, ex.TargetSite.Name, ex.Message));
            }
        }

        public void Start(LoginInfo login)
        {
            this.login = login;
            if (this.login.IsExists())
            {
                this.audio = null;
                DeleteCache.Execute(@"http://cache.monmusugame.com/");
                this.Navigate(GAME_URL);
            }
        }

        public void ScreenShot(string Path)
        {
            try
            {
                if (this.Document == null) return;

                var document = this.Document.DomDocument as mshtml.HTMLDocument;
                if (document == null) return;

                var swf = this.getFrameById(document, "game_frame")?.getElementById("externalswf") as mshtml.HTMLEmbed;
                if (swf == null) return;

                var width = int.Parse(swf.width);
                var height = int.Parse(swf.height);
                var image = new Bitmap(width, height, PixelFormat.Format24bppRgb);
                var rect = new RECT { left = 0, top = 0, width = width, height = height, };
                var tdevice = new DVTARGETDEVICE { tdSize = 0, };
                using (var graphics = Graphics.FromImage(image))
                {
                    var hdc = graphics.GetHdc();
                    IViewObject viewObject = swf as IViewObject;
                    viewObject.Draw(1, 0, IntPtr.Zero, tdevice, IntPtr.Zero, hdc, rect, null, IntPtr.Zero, IntPtr.Zero);
                    graphics.ReleaseHdc(hdc);
                }
                image.Save(Path, ImageFormat.Png);

            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Exception : {0}.{1} >> {2}", ex.TargetSite.ReflectedType.FullName, ex.TargetSite.Name, ex.Message));
            }
        }

        public bool? IsMute()
        {
            if (this.audio == null) { return null; }
            return this.audio.IsMute();
        }

        public void ToggleMute()
        {
            if (this.audio == null) { return; }
            this.audio.ToggleMute();
        }

        #endregion

    }
}