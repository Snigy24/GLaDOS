# Relatório do Projecto IA - Sims Simulator

Autores:

* [André Santos][AS], nº a21700034
* [Ianis Arquissandas][IA] nº 21700021

### Repartição de Tarefas

O projeto foi elaborado de forma a distribuir uniformemente as tarefas por todos e de modo a todos os elementos do grupo fazerem um pouco de tudo. Todas as tarefas estão documentadas sobre a forma de "commits" no nosso repositório do GitHub. Contudo, existiram certos aspectos em que cada um se focou mais.


André Santos
	- Level design
	- Implementação de area de jogo em unity
	- Implementação da câmera RTS 
	- Pesquisa e relatório
	- Apoio na programação
	- Comentários XML
	- Doxygen

Ianis Arquissandas
	- Level design
	- Pesquisa
	- Programação geral do projecto

Leonardo tavares
	- Pesquisa
	- Introdução e conclusão do relatorio

## Introdução

Agent based simulation é um tema que abrange várias áreas distintas, em que todas têm o objetivo de recolha de informação e observação de comportamentos, como por exemplo: estudos sociais, arqueologia, geografia, Biologia, etc. 

Com o avanço tecnológico das últimas décadas, a possibilidade de criar simulações, que anteriormente não haveria a possibilidade de aplicar, veio ajudar a prever e solucionar muitos dos problemas que existentes na nossa sociedade atual, tais como, os relacionados com a segurança, a economia e até os sociais (PO Siebers, CM Macal, J Garnett, D Buxton and M Pidd, 2010)<a name="ref3">\[3\]</a>.



Sabemos que é bastante importante criar planos de proteção e evacuação, que deverão ser mais eficientes e com uma previsão de várias possibilidades dado o acontecimento de diversos imprevisíveis. Contudo os planos de evacuação associados a estes acontecimentos diferem muito uns dos outros devido aos diferentes cenários e ambientes. A simulação destes acontecimentos em computador com a ajuda de agentes é adaptativa e economicamente favorável. (Santos, G. e Aguirre, B. , 2004)<a name="ref4">\[4\]</a> 


apresentaram uma revisão crítica dos modelos de evacuação de emergência dos métodos de simulação, incluindo flow based, celular autómata e agente-based models. 
Este projeto trata de simulações de eventos, relacionados com emergências na evacuação de uma multidão quando ocorre um desastre, como por exemplo, um incêndio num festival de música de larga escala (SuperBock SuperRock), destinado também a reuniões em massa e no estudo prévio sobre o comportamento das multidões e as suas reações em determinados acontecimentos, que poderão ocorrer de forma natural ou que poderão ser despoletados por algo que não esteja ao alcance do agente evitar. 


Pretende-se também estudar o comportamento de um agente e a sua reação quanto à sua independência, mesmo tendo um comportamento igual ao de outros agentes. Tendo em conta os limites da simulação vamos observar alguns dos diferentes comportamentos que (João E. Almeida, Rosaldo Rosseti e António Leça Coelho, 2013)<a name="ref5">\[5\]</a> analisaram. 

O objetivo principal do agente é deslocar-se a um dos palcos existentes, para que possa ouvir o concerto durante um determinado período, em que este comportamento poderá ser alterado por outro evento natural, como por exemplo, o facto de ficar com fome ou quando se encontra cansado. No caso de haver uma mudança no seu objetivo principal, irá passar para "tratar" desse evento, ou seja, se estiver com fome irá deslocar-se para a área de restauração, se estiver cansado deslocar-se-á para uma zona verde para poder repousar, etc. Estes comportamentos irão ser implementados e terão determinadas regras.
A qualquer altura da simulação vai ser possível gerar uma explosão, seguida de um incêndio, que vai afetar o comportamento dos agentes e fazer com que estes entrem em pânico, choque ou que faleçam, dependendo da sua proximidade com o facto ocorrido. Desta forma, ser-nos-á possível observar quais os comportamentos dos agentes em situação de fuga ou pânico dentro do cenário criado.



## Metodologia

O projeto foi desenvolvido com o motor de jogo [Unity][UNT], na linguagem C#. Implementámos o algoritmo de movimento dinâmico, visto que este precisa de "trabalhar" mais. O movimento dinâmico acelera na direção certa e, á medida que se aproxima do alvo, precisa de acelerar na direção oposta. Este movimento enquadra-se no realismo que se pretende alcançar (Craig Reynolds, 1986)<a name="ref6">\[6\]</a>. Embora a geometria do projeto seja em 3D, o movimento que está a ser implementado é em 2.5D.

### Level Design

Foi criado um espaço semelhante a um festival de musica que contem (figura abaixo):

![Cenario](./Documentos/Imgs/cenario_01.png)


Neste espaço encontra-se presente as seguintes zonas:

- Duas zonas verdes
- Três palcos (sendo que um é o principal)
- Duas zonas de restauração com mesas(cada mesa possui quatro bancos)
- Caminhos principais que são utilizados para o movimento entre zonas
- Três saidas (usadas para evacuação)


### Fine State Machines (FSMs)

Utilizamos *Finite State Machine* (FSM) para implementação das reações dos agentes, fluxograma do estados (fig.1)

![Fluxograma](./Documentos/Imgs/fluxograma_01.png)

Foi colocado apenas três estados, pois foi englobado no mesmo a "fome" e  o"cansaço". Visto que o objectivo principal do agente é assitir aos concertos, todos os agentes começam no estado *Watch*. para ser efectuada uma transição de estado o agente precisa de estar cansado/fome ou em pânico. Se por exemplo tiver fome o agente passa para o estado *Replenish* e após se encontrar satisfeito efectua uma transição novamente para o estado *Watch*. Durante qualquer dos estados já mencionados anteriormente, pretende-se que o modo *Panic* ative imediatamente quando uma explosão ocorra perto do agente, mas não perto o suficiente que possa vir a causar a morte do mesmo.

### NavMesh

Foi efectuada a implementação da *NavMesh* que o [Unity][UNT] fornece devido à sua facilidade e implementação prática. Esta utiliza o algoritmo *A** para a procura de caminhos para os agentes. Foram dados diferentes custos às zonas de modo a permitir que o agente siga pelo o caminho principal durante os seus estados. Nenhum agente passa por cima de um obstáculo (árvores/paredes) o que ajuda quanto ao realismo da simulação. Fundamentalmente, foram implementados dois custos diferentes para toda a área disponivel para geração de caminhos e os respectivos obstáculos

### GridSystem

Utilizamos um sistema que gera uma grelha, para ser usada em cojunto com o sistema de *WayPoints*. Esta é personalizada para cada objecto, em que os agentes se podem deslocar e permanecer. Devido ao facto de cada objecto ter dimensões diferentes, este sistema recebe através de uma lista, o número de objectos que existe no nível e cria a grelha, que através de um dicionário, guarda os pontos gerados anteriormente. Após as grelhas para cada objecto estarem geradas, o *GridSystem* fica responsável por informar quais os pontos que se encontram desocupados para o agente poder ocupá-los. 

No caso das zonas verdes, este sistema utiliza um sistema de valor por ponto, desta forma os agentes ao ocuparem um certo lugar, irão dar um "peso" ao ponto que ocupam e também à sua volta. Este peso é utilizado para que os agentes procurem sempre o que têm menos peso. Utilizando este método, os agentes nas zonas verdes irão "procurar" sempre os lugares mais isolados. No caso das zonas de restauração o agente procura sempre a mesa mais vazia, pecorre a lista das mesas e verifica qual se encontra desocupada para poder ocupa-la.


### WayPoints

Utilizamos um sistema de *Waypoints*, para poder guiar o agente ao seu destino, que funciona em cojunto com o *GridSystem*.
O destino do agente divide-se em duas partes, a primeira parte vai da entrada para um recinto (restauração, palcos e zonas verdes), e assim que este chega à entrada, inicia-se de imediato a segunda parte, em que o sistema irá pecorrer uma lista de *waypoints* correspondente a área em questão, como por exemplo, a lista de pontos da zona verdes, que irá indicar ao agente para qual ponto se deverá deslocar de seguida.

Este sistema possui uma variável booleana, que verifica se o lugar já se encontra ocupado/obstruido, a posição X e Y na grid. De modo a poder ser visualizado no editor, criou-se um método que permite ver os respectivos waypoints inseridos na grelha.

Na imagem abaixo, apresentams um fluxograma simples de como estes dois sistems interagem (WayPointSystem e GridSystem). Embora não esteja representado como um ciclo, este exemplo ocorre sempre que o agente muda o seu estado entre *Watch* e *Replenish*.

![Fluxograma](./Documentos/Imgs/WayPoint_01.png)


### Valores Parametrizáveis

Numero de agentes (minimo): 100
Velocidade normal do agente: 3.5
Velocidade em pânico: velocidade normal x 2
Velocidade no raio secundário da explosão: velocidade normal / 2
Tempo minimo a visualizar o concerto: 10sec

É possivel observar o número de agentes vivos, os que conseguiram escapar e os que faleceram.O numero máximo de agentes poderá ser alteado no decorrer da simulação. 

### Agentes 

Cada agente tem os seus próprios atributos como a fome, a exaustão e a necessidade/gosto de visualizar o concerto. No inicio de cada simulação o valor para cada um deles é diferente, sendo gerado de forma aleatória. Devido ao factor aleatório, cada agente tem o seu comportamento individualizado perante um comportamento geral do grupo.

Como o "objectivo" principal do agente é deslocar-se para um dos três palcos, ao chegar a um deles, fica durante um certo período, que é determinado por uma variável que tem um valor mínimo de 10sec e não existe um valor máximo, ou seja, cada segundo que passa após os 10 segundos, a probablidade de abandonar o palco é maior. De acordo com a necessidade de cada agente, deslocar-se-ão para um ponto de necessidade, se sentir fome desloca-se para a zona de restauração, se estiver cansado para uma zona verde. O agente irá procurar a zona mais isolada ou mesa vazia em cada uma das situações.

![GeralAgente](./Documentos/Imgs/GeralAgente_01.png)

(fig) Comportamento do agente sem pânico


## Resultados e discussão

• Apresentação dos resultados, salientando os aspetos mais interessantes que
observaram na simulação, em particular se observaram comportamento emergente,
isto é, comportamento que não foi explicitamente programado nos agentes.

• Caso tenham experimentado diferentes parâmetros (no de saídas, no de agentes,
tempos de espera entre zonas, velocidades, taxa de propagação dos incêndios,
taxa de propagação do pânico, etc), e/ou quantidade e/ou local das
explosões, podem apresentar quadros, tabelas e/ou gráficos com informação
que considerem importante.


• Na parte da discussão, devem fazer uma interpretação dos resultados que
observaram, realçando quaisquer correlações que tenham encontrando entre
estes e as parametrizações que definiram, bem como resultados inesperados,
propondo hipóteses explicativas.

## Conclusão 

Nesta secção devem relacionar o que foi apresentado na introdução, nomeadamente
o problema que se propuseram a resolver, com os resultados que obtiveram,
e como o vosso projeto e a vossa abordagem se relaciona no panorama
geral da pesquisa que efetuaram sobre simulação de pânico em multidões.
• Uma pessoa que leia a introdução e conclusão do vosso relatório deve ficar
com uma boa ideia daquilo que fizeram e descobriram, embora sem saber os
detalhes.


## Referências

*   <a name="ref1">\[1\]</a> Whitaker, R. B. (2016). **The C# Player's Guide**
    (3rd Edition). Starbound Software.

*   <a name="ref2">\[2\]</a> Millington, I. (2019). **AI for Games**
    (3rd Edition). CRC Press.

*   <a name="ref3">\[3\]</a> Siebers, P. Macal, Garnett, J. Buxton, D. and Piddm M. (2010). **Discrete-event simulation is dead, long live agent-based simulation!**
    Obtido de [https://www.tandfonline.com/doi/full/10.1057/jos.2010.14?casa_token=D3XtqIzBk7IAAAAA%3AoRD5pRk6fi8-pgD58AURxXv4CXDPAPDBHk_LEq2wmTo_02JJ6cx-Yqvng7MQ0RI44hlLLS7GeWcN8N0&][ART3].

*   <a name="ref4">\[4\]</a> Santos, G. & Aguierre, B. (2004). **A critical review of emergency evacuation simulation models**
    Obttido de [http://udspace.udel.edu/handle/19716/299][ART2].

*   <a name="ref5">\[5\]</a> Almeida, J. Rosaldo, J. Rosseti, F. Coelho, A. (2013). **Crowd Simulation Modeling Applied to Emergency and Evacuation Simulations using Multi-Agent Systems**
    Obtido de [https://arxiv.org/abs/1303.4692][ART4].


*   <a name="ref6">\[6\]</a> Reynolds, Craig (1987). **Flocks, herds and schools: A distributed behavioral model**
    Obtido de [https://dl.acm.org/citation.cfm?doid=37401.37406][ART1].


## Metadados


Curso:

* [Licenciatura em Videojogos][LV]

Instituição: 

* [Universidade Lusófona de Humanidades e Tecnologias][ULHT]

[AS]:https://github.com/Snigy24
[IA]:https://github.com/Insoel
[ULHT]:https://www.ulusofona.pt/
[LV]:https://www.ulusofona.pt/licenciatura/aplicacoes-multimedia-e-videojogos
[UNT]:https://unity.com/
[ART1]:https://dl.acm.org/citation.cfm?doid=37401.37406
[ART2]:http://udspace.udel.edu/handle/19716/299
[ART3]:https://www.tandfonline.com/doi/full/10.1057/jos.2010.14?casa_token=D3XtqIzBk7IAAAAA%3AoRD5pRk6fi8-pgD58AURxXv4CXDPAPDBHk_LEq2wmTo_02JJ6cx-Yqvng7MQ0RI44hlLLS7GeWcN8N0&
[ART4]:https://arxiv.org/abs/1303.4692



Description of the developed solution, namely the implemented algorithm and the chosen static evaluation function (heuristic).

    Diagrams or schemes that aid, enhance and/or simplify the description will have positive influence in the final grade.



Descrição da solução

Para a nossa implementação de *Artificial Inteligence* no projecto *ColorShapeLinks AI*,
 utilizamos o algoritmo *Minimax* (*Ver secção Minimax para mais detalhes*) 
que é responsavel para avaliar as jogadas possiveis e retornar a melhor opção para 
a nossa *AI*. Devido a isso, foi criado três classes, *G04GLaDOSAI* que herda da classe *AIPlayer*, *G04GLaDOSAIThinker* que implementa a interface *IThinker* e `G04GLaDOSStaticEvaluation` (*ver secção da heuristica mais abaixo*).


Referente a classe `G04GLaDOSAI`, esta classe pertimite que a nossa *AI* (GLaDOS) 
seja encontrada no jogo, de modo a competir contra um jogador ou outra *AI*. Isto
 é essencial viso que este projecto tem como finalidade correr apenas no editor. 
Esta classe implementa a propriedade *PlayerName* que indica que a nossa *AI* dá 
o nome de GLaDOS e a propriedade *Thinker* que retorna uma instância da classe 
que implementa `IThinker`.

Já na classe `G04GLaDOSAIThinker` é onde se encontra implementado o algoritmo 
Minimax com cortes alfa e beta, para poder verificar qual é a melhor e proxima jogada que 
a nossa *AI* irá efectuar. 
Existe variaveis para poder indicar a profundidade de procura máxima, as cores e
 formas de cada jogador(AI ou oponente) e uma instãncia para poder usar a 
heuristica que se encontra noutra classe.
*ver secção função de avaliação estática para mais detalhes*.

	

A nossa AI escolhe a melhor jogada através do método `Think(Board board, CancellationToken ct)`
Este método começa por passar uma cópia do estado do tabuleiro
e verifica qual é a forma e cor do adversário juntamente com a própria cor e forma.

Para iniciação da verificação de qual a melhor jogada é criado uma variável que
 indica e guarda o melhor score com o valor mais baixo possivel, ou seja menos infinito.

 De seguida entra num ciclo `for`, 
que permite pecorrer todas as jogadas do tabuleiro em busca do melhor score. Devido a isso 
o ciclo inicia fazendo algumas verificações, se excedeu o tempo que tem para efectuar a jogada, se 
ainda tem uma determinada peça e ignora as colunas que já se encontram cheias. Após a "aprovação" 
das verificações mencionadas acima, é testado uma jogada e chama-se o método que contem o 
*Minimax* para efectuar a validação do *score*. Após termos o *score* desfaz-se a jogada
e caso esse *score* seja melhor que o actual, guarda-se o novo *score*, caso contrário não é alterado.
Antes do tempo finalizar ou ser efectuada a verificação das colunas o método retorna a melhor jogada.

![CicloFor](./Imgs/For.png)

*(fig) Ciclo for em diagrama*



Minimax

Como mencionado em cima, o método `private int Minimax(Board board, int depth, int alpha, int beta, bool maximizingPlayer, CancellationToken ct)` é responsável por verificar todas as jogadas possiveis até à profundida indicada (`int depth`). 

Se chegar a profundidade indicada ele retorna um valor da função de avaliação estática que reflecte o estado do tabuleiro. Caso existe um vencedor, o Minimax retorna o valor máximo de 1000 (AI) ou minímo -1000 (oponente), em caso de empate retorna o valor de -500 que é entre o 0 e -1000. 

heuristica

Tem como função avaliar o estado do tabuleiro actual, ou seja se o jogador estiver perto da vitória têm um maior pontuação e vice-versa.

A implementação da heuristaica foi efectuada numa classe `G04GLaDOSStaticEvaluation` , foi efectuada da seguinte forma para a parte da AI:

- Verificar todas as peças da GLaDOS em comparação com todos os corredores de vitoria no tablueiro possives.

- Por cada peça no tablueiro é aumentado um ponto, por cada peça consequente em linha (horizontal/vertical/diagonal), esse ponto é multiplicado por o numero de peças em "linha". Exemplo: 3 peças seguidas =  1x2x3.

- Se for encontrado uma peça do oponente, é retirado a pontuação dessas peças em linha.

Após calcular o valor da pontuação da nossa AI é calculada o valor do tablueiro para o oponente (utilizando a mesma implementação), isto é, quantas peças o inimigo tem em linha.
No final do método é retornado a pontuação da GLaDOS (AI) subtraido pela a pontuação do oponente.

![CicloFor](./Imgs/Heur.png)