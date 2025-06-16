using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MatchGame
{
    using System.Windows.Threading;

    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        int tempoDecorrido; //Variavel para armazenar tempo do jogo
        int emojiEncontrados;//Variavel para armazenar pares de emojis encontrados
        public MainWindow()
        {
            InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(.1);
            timer.Tick += Temporizador;
            SetUpGame();
        }

        private void Temporizador(object sender, EventArgs e)
        {
            tempoDecorrido++;// Acrescenta uma unidade ao tempo
            timeTextBlock.Text = (tempoDecorrido / 10F).ToString("0.0s");//Escreve o tempo na variavel e imprime na tela
            if (emojiEncontrados == 8)//Verifica se jogo acabou - se acabou...
            {
                timer.Stop();//...Para a contagem do tempo
                timeTextBlock.Text = timeTextBlock.Text + " - Jogar Novamente?";//...Imprime a mensagem na tela, se for clicado insere o evento do clique
            }
        }

        private void SetUpGame()
        {
            //Cria uma lista com 8 pares de emojis
            List<string> animalEmoji = new List<string>()
            {
                "🐙", "🐙",
                "🐟", "🐟",
                "🐘", "🐘",
                "🐳", "🐳",
                "🐪", "🐪",
                "🦒", "🦒",
                "🦘", "🦘",
                "🐎", "🐎",
            };
            //Cria uma instância da classe Random
            Random random = new Random();
            //laço para varrer todo o grid verificando TextBlock
            foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
            {
                if(textBlock.Name != "timeTextBlock")//Validação para não escrever no tempo
                {
                    textBlock.Visibility = Visibility.Visible;
                    int index = random.Next(animalEmoji.Count);//cria um numero aleatório de 0 até a quantidade de emoji
                    string nextEmoji = animalEmoji[index];//insere o animalEmoji da posição recebida aleatória na variavel
                    textBlock.Text = nextEmoji;//insere esse emoji no TextBlock
                    animalEmoji.RemoveAt(index);//Remove esse emoji para não haver repetições
                    
                }
            }
            //Aqui todos os emojis foram inseridos na tela
            timer.Start();//Inicializa o tempo
            tempoDecorrido = 0;//inicializa a variavel em 0
            emojiEncontrados = 0;//inicializa o valor de emojis em zero - para não haver sujeira do jogo anterior
        }

        TextBlock ultimoBlocoDeTextoClicado; //Variavel criado para armazenar botão clicado
        bool encontrarCorrespondente;//Variavel criada para fazera validação
        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;//Instaciado o objeto TextBlock
            if(encontrarCorrespondente == false)// Se o valor do encontrarCorrespondente for falso, que no primeiro 
            {
                textBlock.Visibility = Visibility.Hidden;//esconda o valor do emoji clicado
                ultimoBlocoDeTextoClicado = textBlock;//ultimoBlocoTextoClicado recebe o valor do emoji clicado
                encontrarCorrespondente = true;//agora a variavel de validação criada passa ser verdadeira
            }
            else if(textBlock.Text == ultimoBlocoDeTextoClicado.Text)//verifica se o valor armazenado no clique anterior é igual ao clicado recente
            {
                emojiEncontrados++;//se for igual o clicado, acrescenta uma unidade nessa variavel
                textBlock.Visibility = Visibility.Hidden;//esconde o emoji clicado
                encontrarCorrespondente= false;//alterna a variavel para iniciar a validação novamente
                
            }
            else//se o ultimo evento clicado for diferente
            {
                ultimoBlocoDeTextoClicado.Visibility = Visibility.Visible;//o primeiro evento clicado fica visivel novamente
                encontrarCorrespondente = false;//alterna a variavel para iniciar a validação novamente
            }
        }

        private void timeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)//método criado para quando terminar o jogo, ao ser clicado na mensagem reinicia o jogo
        {
            if (emojiEncontrados == 8)//valida para ver se o total de pares encontrado é igual a 8... se for
            {
                SetUpGame();//..reinicializa o jogo
            }
        }
    }
}
