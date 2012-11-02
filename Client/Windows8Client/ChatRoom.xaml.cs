using System;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Myers.NovCodeCamp.Client.Windows8.ChatServiceReference;

namespace Myers.NovCodeCamp.Client.Windows8
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class ChatRoom : Myers.NovCodeCamp.Client.Windows8.Common.LayoutAwarePage
    {
        private ChatServiceClient _svcClient;
        private string _username;

        public ChatRoom()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Chat client callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReceiveMessage(object sender, RecieveMessageReceivedEventArgs e)
        {
            // create the paragraph
            Paragraph p = new Paragraph();
            Run rnMyText = new Run();
            p.FontWeight = FontWeights.Bold;

            // if the message is from the currently logged in user, then set the color to gray
            if (e.msg.From == _username)
            {
                p.Foreground = new SolidColorBrush(Colors.Gray);
                rnMyText.Text = string.Format("{0} (me): {1}", e.msg.From, e.msg.MessageText);
            }
            else
            {
                p.Foreground = new SolidColorBrush(Colors.Green);
                rnMyText.Text = string.Format("{0}: {1}", e.msg.From, e.msg.MessageText);
            }

            // add the text to the paragraph tag
            p.Inlines.Add(rnMyText);

            // add the paragraph to the rich text box
            rtbChatLog.Blocks.Add(p);
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState) { }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // retrieve the username from the payload
            _username = (string)e.Parameter;

            // setup the service client
            _svcClient = new ChatServiceClient();
            _svcClient.RecieveMessageReceived += ReceiveMessage;

            // login to the service
            _svcClient.LoginAsync(_username).Wait();
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState) { }

        /// <summary>
        /// Handle the user clicking the send button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSendChatMessage_Click(object sender, RoutedEventArgs e)
        {
            // create the chat message and send it across the wire
            var msg = new ChatMessage() { From = _username, MessageText = txtUserMessage.Text };
            _svcClient.SendMessageAsync(msg);

            // clear out the message text box
            txtUserMessage.Text = "";
        }
    }
}
