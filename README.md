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

Os resultados da implementação mostraram que o *Naive Bayes* é eficaz em prever os movimentos dos *NPCs*, resultando numa taxa de erro de apenas 0,5%. A precisão do classificador na decisão dos movimentos dos *NPCs* melhorou significativamente a jogabilidade e a complexidade do *Maze Chase*. [[1]](https://ieeexplore.ieee.org/abstract/document/9034640)

##### **Comparação com o nosso projeto**

- **Flexibilidade e Parametrização:**
    - No *Maze Chase*, o classificador *Naive Bayes* é utilizado para calcular os movimentos dos *NPC*s com base em vários parâmetros dinâmicos do jogo, como a distância ao jogador e a quantidade de moedas. No nosso projeto, o *Naive Bayes* é empregado para aprender e replicar os movimentos e ações do jogador humano, ajustando-se às ações específicas do jogador em tempo real.

- **Interação e "humanização" da IA:**
    - No *Maze Chase*, o sistema multi-agente permite que os *NPCs* cooperem e reajam de forma coordenada, aumentando a complexidade e o realismo da perseguição no jogo. No nosso projeto, o foco está na replicação precisa das ações do jogador humano por um agente de IA, criando uma interação realista e imersiva entre o jogador e o agente para um comportamento mais "humano".

## Metodologia

A implementação deste trabalho foi feita em 2D, através do *game engine* Unity. O jogo ocorre numa grelha onde o personagem Luso se move, com um certo número de jogadas, para apanhar lixo. O objetivo é maximizar a pontuação ao apanhar o máximo de lixo possível, e evitar movimentos que resultem em penalizações.

### Grelha

A grelha é composta por uma matriz bidimensional que representa diferentes tipos de células, incluindo paredes, células vazias e células com lixo.

A grelha é inicializada com um tamanho específico, que pode ser definido pelo utilizador através do editor do Unity. Este tamanho (*TrashGame.cs - gridSize*) determina a dimensão da grelha de jogo. Assim, se o tamanho da grelha definido pelo utilizador for 8, o tamanho total da grelha será 8x8.

Durante a inicialização do jogo, a grelha é preenchida com paredes nas bordas e, em seguida, as células internas são atribuídas como vazias ou contendo lixo com base na probabilidade definida pelo utilizador (*TrashGame.cs - chanceForTrash*).

#### **Parâmetros Configuráveis**

Os seguintes parâmetros da grelha podem ser definidos no editor do Unity:

- **Tamanho da Grelha (*TrashGame.cs - gridSize*)**: Determina o número de células na grelha. O tamanho pode ser ajustado para diferentes valores, adequando-se às necessidades do jogo.
- ***Seed* Aleatória (*TrashGame.cs - seeded* e *TrashGame.cs - seed*)**: Permite a utilização de uma *seed* específica para a geração aleatória da grelha, o que garante que a mesma configuração possa ser replicada em diferentes execuções do jogo.
- **Probabilidade de Lixo (*TrashGame.cs - chanceForTrash*)**: Define a probabilidade de uma célula conter lixo, expressa como uma percentagem de 0 a 100.

![PLACEHOLDER PARA PARÂMETROS DA GRELHA]()

#### **Modelo da Grelha**

![PLACEHOLDER PARA MODELO DA GRELHA]()

### Jogador

O jogador pode ser controlado de duas formas: pelo jogador humano ou por um agente de IA. Ambas as formas partilham algumas funcionalidades comuns, mas diferem na forma como as ações são executadas e controladas. O jogador ao inicio é colocado numa posição aleatória na grelha, e o seu objetivo é apanhar o lixo presente nas células da grelha e fazer o máximo de pontuação possível.

Ambos os jogadores, humano e IA, podem realizar as seguintes ações:

- **Mover para a Direita:** Luso move-se uma célula para a direita.
- **Mover para a Esquerda:** Luso move-se uma célula para a esquerda.
- **Mover para Cima:** Luso move-se uma célula para cima.
- **Mover para Baixo:** Luso move-se uma célula para baixo.
- **Apanhar Lixo:** Luso apanha o lixo presente na célula onde se encontra.
- **Permanecer Parado**: Luso não se move e mantém a sua posição atual.
- **Mover Aleatoriamente:** Luso realiza um movimento aleatório entre as direções disponíveis, sem a possibilidade de ficar parado, pois logicamente só se usaria random quando estamos rodeados de tiles vazias então o *stay* não faria sentido neste caso

Estas ações são registadas e atualizadas no jogo. A execução das ações influencia a pontuação do jogo, esta pontuação pode ser também negativa, caso o jogador realize ações que resultem em penalizações.

- **Mover-se contra uma Parede:** Penaliza o jogador com -5 pontos.
- **Apanhar Lixo:** Recompensa o jogador com 10 pontos.
- **Tentar Apanhar Lixo numa Célula Vazia:** Penaliza o jogador com -1 ponto.

O movimento do jogador pode ser instantâneo ou animado, dependendo da configuração definida pelo utilizador (*TrashGame.cs - playerInstantMovement*). A opção de movimento instantâneo permite que o jogador se mova sem animações de transição. O jogador pode ainda definir o tempo da animação de movimento (*PlayerMovement.cs - howMuchToMovePerFrame*) definindo o número de frames necessários para completar um movimento, quanto maior o número de frames, mais rápida será a animação.

#### **Parâmetros Configuráveis**

No editor do Unity, podem ser configurados os seguintes parâmetros que afetam o comportamento do jogador:

- **Máximo de Movimentos (*TrashGame.cs - maxMoves*):** Define o número máximo de movimentos que o jogador pode realizar numa sessão de jogo.
- **Movimento Instantâneo do Jogador (*TrashGame.cs - playerInstantMovement*):** Permite que o movimento do jogador seja instantâneo, sem animações de transição.
- **Tempo de Animação de Movimento (*PlayerMovement.cs - howMuchToMovePerFrame*):** Define o número de frames necessários para completar um movimento, ajustando a velocidade da animação.

![PLACEHOLDER PARA PARÂMETROS DO JOGADOR]()

#### **Modelo do Luso**

![PLACEHOLDER PARA MODELO DO LUSO]() [2](https://trevor-pupkin.itch.io/tech-dungeon-roguelite)

#### **Modelo do lixo**

![PLACEHOLDER PARA MODELO DO LIXO]() [3](https://btl-games.itch.io/trash-and-junk-asset-pack)

### Jogador Humano

O jogador humano controla o Luso utilizando as teclas WASD para movimentação e a tecla E para apanhar lixo. A tecla Espaço permite que o jogador permaneça parado, enquanto a tecla R faz com que o jogador se mova aleatoriamente numa das direções disponíveis.

### Jogador IA

Enquanto o jogador humano controla Luso, o agente de IA regista as ações realizadas em cada situação específica da grelha. Luso está numa vizinhança de *Von Neumann* (4 vizinhos + célula atual), o que significa que pode ver a sua célula atual e as células adjacentes (cima, baixo, esquerda, direita). Estas células podem estar em três estados: vazia, com lixo ou parede, resultando em 162 situações diferentes possíveis.

![PLACEHOLDER PARA VIZINHANÇA DE VON NEUMANN]()

Essencialmente, o que o classificador *Naive Bayes* faz, neste caso, é, para cada uma destas situações, observar qual é a ação efetuada pelo humano, tentando depois replicar esse comportamento quando for a sua vez de jogar, por isso é importante que o jogador humano jogue um X número de vezes para a IA aprender com o seu comportamento.

Quando a IA está a jogar, utiliza as probabilidades calculadas pelo classificador *Naive Bayes* para decidir a ação a tomar em cada passo. A IA realiza estas ações automaticamente com uma pausa definida pelo utilizador (*PlayerMovement - timeForEachAITurn*) entre cada passo, para "imitar" o comportamento previamente observado do jogador humano.

#### **Parâmetros Configuráveis**

Os seguintes parâmetros da IA podem ser definidos no editor do Unity:

- **Número de Tempo entre Turnos da IA (*PlayerMovement.cs - timeForEachAITurn*):** Define o tempo de pausa entre cada ação da IA, permitindo ajustar a velocidade de execução da IA.

![PLACEHOLDER PARA PARÂMETROS DA IA]()

### UI

A UI permite iniciar o jogo, escolher o modo de jogo (jogador humano ou IA), e visualizar a pontuação e outras informações relevantes durante a sessão de jogo.

O menu principal, apresenta botões que permitem ao jogador iniciar o jogo com um jogador humano ou com a IA, bem como sair do jogo.

#### **Botões de Início de Jogo:**

- **Iniciar com Jogador Humano (*ButtonsManager - StartWithHuman*):** Inicia o jogo com controlo humano. Define AI como *false* e inicia o jogo.
- **Iniciar com IA (*ButtonsManager - StartWithAI*):** Inicia o jogo com controlo da IA. Define AI como *true* e inicia o jogo.
- **Botão de Sair (*ButtonsManager - QuitGame*):** Fecha o jogo se estiver a ser executado como uma aplicação *standalone*, ou termina a execução se estiver no editor do Unity.

![PLACEHOLDER PARA UI NO MENU PRINCIPAL]()

Os botões do menu são desativados uma vez que o jogo começa, prevenindo interferências durante a sessão de jogo. Dentro o é exibido a pontuação, o número de movimentos restantes durante o jogo, movimentos feitos pelo jogador / movimentos máximos e o número de observações feitas pela IA.

![PLACEHOLDER PARA UI DENTRO DE JOGO]()

Quando o jogo termina, a UI exibe a pontuação final do jogador, destacando o desempenho obtido, juntamente com as 6 melhores pontuações obtidas naquela sessão de jogo. E voltam a ser ativados os botões do menu principal.

![PLACEHOLDER PARA UI DE FIM DE JOGO]()

## Resultados e discussão

### Resultados

A melhor pontuação possível em 20 jogadas é 100 pontos, considerando que o jogador apanha uma peça de lixo em cada duas jogadas (uma para se mover e outra para apanhar o lixo). Para atingir essa pontuação, é necessário que haja sempre pelo menos um lixo numa das células vizinhas à célula atual do jogador.

Após um número de observações adequado, tendo o jogador estado pelo menos uma vez em cada célula interna da grelha, a IA demonstrou capacidade de imitar o comportamento humano, alcançando pontuações comparáveis. Em alguns casos, a IA conseguiu igualar a pontuação máxima de 100 pontos.

Os resultados mostram que, com observações suficiente, a IA pode replicar eficazmente as decisões do jogador humano, atingindo a pontuação máxima possível. Este desempenho valida a eficácia do classificador *Naive Bayes* utilizado no projeto.

### Discussão

## Conclusão

## Agradecimentos

## Referências

- [1] Zohro’iyah, H., Nasution, S., & Nugrahaeni, R. (2020, March 16). Determining NPC behavior in Maze Chase game using Naïve Bayes algorithm | IEEE conference publication | IEEE xplore. https://ieeexplore.ieee.org/abstract/document/9034640 
- [2] Pupkin. (2021, September 23). Tech Dungeon: Roguelite - Asset Pack by pupkin. itch.io. https://trevor-pupkin.itch.io/tech-dungeon-roguelite 
- [3] BTL Games. (2021, September 10). Trash and Junk Asset Pack by BTL Games. itch.io. https://btl-games.itch.io/trash-and-junk-asset-pack