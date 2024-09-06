using DSharpPlus;
using DSharpPlus.Entities;
using KeyAuth;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dmall
{
    public partial class Form1 : Form
    {

        public static api KeyAuthApp = new api(
        name: "dmall", // Application Name
        ownerid: "9lHPPXiR0P", // Owner ID
        secret: "f204b87076f54bbec66ea00ac5ec84842a5e57dca3a4a8fa58e2ba65cd74c435", // Application Secret
        version: "1.0" // Application Version /*
                       //path: @"Your_Path_Here" // (OPTIONAL) see tutorial here https://www.youtube.com/watch?v=I9rxt821gMk&t=1s

         );

        private DiscordClient discord;
        private CancellationTokenSource cts;
        private Random random;
        public Form1()
        {
            random = new Random();
            InitializeComponent();
        }

        private void Log(string message)
        {
            if (this.logTextBox.InvokeRequired)
            {
                this.logTextBox.Invoke(new Action(() => this.logTextBox.AppendText(message + Environment.NewLine)));
            }
            else
            {
                this.logTextBox.AppendText(message + Environment.NewLine);
            }
        }

        private async void startdm_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tokentextbox.Text))
            {
                Log("Erreur : le token est vide.");
                return;
            }

            cts = new CancellationTokenSource();

            discord = new DiscordClient(new DiscordConfiguration()
            {
                Token = tokentextbox.Text,
                TokenType = TokenType.Bot
            });

            try
            {
                await discord.ConnectAsync();
                Log("Bot connecté à Discord");

                var guildId = ulong.Parse(guiIDtextbox.Text);
                var guild = await discord.GetGuildAsync(guildId);
                Log($"Connecté au serveur : {guild.Name}");

                var members = await guild.GetAllMembersAsync();
                Log($"Nombre total de membres : {members.Count}");

                foreach (var member in members)
                {
                    if (cts.Token.IsCancellationRequested)
                    {
                        Log("Envoi des messages arrêté par l'utilisateur");
                        break;
                    }

                    if (!member.IsBot)
                    {
                        try
                        {
                            var mentionMessage = $"<@{member.Id}> {messagetextbox.Text}";
                            await member.SendMessageAsync(mentionMessage);
                            Log($"Message envoyé à {member.Username}#{member.Discriminator} avec mention");
                        }
                        catch (Exception ex)
                        {
                            Log($"Échec de l'envoi du message à {member.Username}#{member.Discriminator} : {ex.Message}");
                        }
                    }
                    int delay = random.Next(10, 501);
                    await Task.Delay(delay);
                }

                Log("Envoi des messages terminé.");
            }
            catch (Exception ex)
            {
                Log($"Erreur de connexion : {ex.Message}");
            }
        }


        private void stopdm_Click(object sender, EventArgs e)
        {
            cts?.Cancel();
            Log("sending messages canceled.");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            KeyAuthApp.init();
            KeyAuthApp.check();
        }

        private void label2_Click(object sender, EventArgs e)
        {
           
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            KeyAuthApp.license(licensetextbox.Text);
            if (KeyAuthApp.response.success)
            {
                MessageBox.Show("License", "Login", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                guna2Panel2.Hide();
            }
            else
            {
                Application.Exit();
            }
        }
    }
}