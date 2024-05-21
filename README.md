# Trash Picker IA

## Autoria

- António Rodrigues, 22202884
- Rafael José, 22202078

## Divisão

## Introdução

O nosso projeto visa criar um jogo em Unity onde o personagem Luso, um robô, apanha lixo numa grelha quadrangular, a qual o utilizador pode definir o seu tamanho. O Luso pode ser controlado por um humano ou por um agente de IA que aprende com o comportamento humano, por isso é importante que o jogador jogue algumas vezes para a IA aprender com o seu comportamento antes de a colocar a jogar sozinha, para obter os melhores resultados possíveis.

Para implementar o comportamento da IA, utilizámos a biblioteca *LibGameAI.NaiveBayes*, fornecida pelo professor Nuno Fachada, para implementar um classificador *Naive Bayes*. Este classificador observa e regista os movimentos e ações do jogador humano e, posteriormente, utiliza essas observações para tomar decisões quando controlado pela IA.

Os nossos objetivos incluíram desenvolver um jogo funcional com mecânicas simples e intuitivas e implementar um agente de IA que aprende e imita eficazmente o comportamento humano.

### Artigo de investigação

#### *Determining NPC Behavior in Maze Chase Game using Naive Bayes Algorithm*

Este trabalho explora a aplicação do classificador *Naive Bayes* para determinar o comportamento de *NPCs* (*Non-Player Characters*) num jogo de perseguição chamado *Maze Chase*. O *Maze Chase* é um jogo onde o jogador deve evitar ser capturado por vários *NPCs*. A implementação proposta utiliza o *Naive Bayes* para calcular a probabilidade dos movimentos dos *NPCs* com base em diferentes parâmetros do jogo, como a distância ao jogador, o número de moedas numa zona e a distância ao centróide da zona.

Os *NPC*s no *Maze Chase* possuem características distintas e utilizam um sistema multi-agente para se comunicarem e coordenarem as suas ações. Este sistema multi-agente, parte da inteligência artificial, permite que os *NPCs* cooperem entre si para capturar o jogador de forma eficiente. A metodologia baseia-se na recolha de dados dos *NPCs* e do jogador em várias zonas do labirinto, utilizando esses dados para treinar o classificador *Naive Bayes*, que então determina a direção e os movimentos dos *NPCs*.

Os resultados da implementação mostraram que o *Naive Bayes* é eficaz em prever os movimentos dos *NPCs*, resultando numa taxa de erro de apenas 0,5%. A precisão do classificador na decisão dos movimentos dos *NPCs* melhorou significativamente a jogabilidade e a complexidade do *Maze Chase*.

##### **Comparação com o nosso projeto**

- **Flexibilidade e Parametrização:**
    - No *Maze Chase*, o classificador *Naive Bayes* é utilizado para calcular os movimentos dos *NPC*s com base em vários parâmetros dinâmicos do jogo, como a distância ao jogador e a quantidade de moedas. No nosso projeto, o *Naive Bayes* é empregado para aprender e replicar os movimentos e ações do jogador humano, ajustando-se às ações específicas do jogador em tempo real.

- **Interação e "humanização" da IA:**
    - No *Maze Chase*, o sistema multi-agente permite que os *NPCs* cooperem e reajam de forma coordenada, aumentando a complexidade e o realismo da perseguição no jogo. No nosso projeto, o foco está na replicação precisa das ações do jogador humano por um agente de IA, criando uma interação realista e imersiva entre o jogador e o agente para um comportamento mais "humano".

## Metodologia

## Resultados e discussão

## Conclusão

## Agradecimentos

## Referências
