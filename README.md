# Relatório do Projecto IA - Sims Simulator

Autores:

* [André Santos][AS], nº a21700034
* [Ianis Arquissandas][IA] nº 21700021

### Repartição de Tarefas

O projeto foi elaborado de forma a distribuir uniformemente as tarefas por todos
e de modo a todos os elementos do grupo fazerem um pouco de tudo. Todas as tarefas
estão documentadas sobre a forma de "commits" no nosso repositório do GitHub 
*(pedir acesso visto encontra-se em privado)*. 
Contudo, existiram certos aspectos em que cada um se focou mais.


André Santos
	- Relatório
	- Implementação de algoritmo Minimax
	- Função de avaliação estática

Ianis Arquissandas
	- Relatório
	- Melhoramento do algoritmo Minimax com cortes alfa e beta
	- Função de avaliação estática


## Descrição da Solução

O projeto foi desenvolvido com o motor de jogo [Unity][UNT], na linguagem C#. 
Para a nossa implementação de *Artificial Inteligence* no projecto *ColorShapeLinks AI*,
 utilizamos o algoritmo `Minimax` *(Ver secção Minimax para mais detalhes)* com 
 cortes alfa e beta, visto que este permite saltar ramos inteiros da árvore e
 torna o Minimax normal 10x mais rápido.
 
O *Minimax* é responsavel para avaliar as jogadas possiveis e retornar a melhor
opção para a nossa *AI*. Devido a isso, foi criado três classes, `G04GLaDOSAI` 
que herda da classe `AIPlayer`, `G04GLaDOSAIThinker` que implementa a interface 
`IThinker` e `G04GLaDOSStaticEvaluation` *(ver secção da heuristica mais abaixo)*.

Referente a classe `G04GLaDOSAI`, esta classe pertimite que a nossa *AI* (GLaDOS) 
seja encontrada no jogo, de modo a competir contra um jogador ou outra *AI*. Isto
 é essencial viso que este projecto tem como finalidade correr apenas no editor. 
Esta classe implementa a propriedade *PlayerName* que indica que a nossa *AI* dá 
o nome de GLaDOS e a propriedade *Thinker* que retorna uma instância da classe 
que implementa `IThinker`.

Já na classe `G04GLaDOSAIThinker` é onde se encontra implementado o algoritmo 
Minimax com cortes alfa e beta, para poder verificar qual é a melhor e proxima 
jogada que a GLaDOS irá efectuar. Existe variaveis para poder indicar a profundidade
de procura máxima, as cores e formas de cada jogador(AI ou oponente) e uma 
instância para poder usar a heurística que se encontra noutra classe.
*ver secção função de avaliação estática para mais detalhes*.
	
A nossa AI escolhe a melhor jogada através do método `Think(Board board, CancellationToken ct)`
Este método começa por passar uma cópia do estado do tabuleiro e verifica qual 
é a forma e cor do adversário juntamente com a própria cor e forma. Para iniciação
da verificação de qual a melhor jogada é criado uma variável que indica e guarda 
o melhor score com o valor mais baixo possivel, ou seja menos infinito.

De seguida entra num ciclo `for`, que permite pecorrer todas as jogadas do 
tabuleiro em busca do melhor score. Devido a isso o ciclo inicia fazendo algumas
verificações, se excedeu o tempo que tem para efectuar a jogada, se ainda tem uma
determinada peça e ignora as colunas que já se encontram cheias. Após a "aprovação" 
das verificações mencionadas acima, é testado uma jogada e chama-se o método que
contem o *Minimax* para efectuar a validação do *score*. Após termos o *score* 
desfaz-se a jogada e caso esse *score* seja melhor que o actual, guarda-se o novo
*score*, caso contrário não é alterado. Antes do tempo finalizar ou ser efectuada
a verificação das colunas o método retorna a melhor jogada.

![CicloFor](./Imgs/For.png)

*(fig) Ciclo for em diagrama*


### Minimax 

O princípio do Minimax é descer os *nós* da *Game Tree* até chegar á posição final
do tabuleiro, identificando se o jogador perdeu, empatou ou ganhou. Após isso, 
o algoritmo sobe um *nó* e identifica de quem é o turno (jogador(AI) ou oponente).
Caso for o turno do oponente, o algoritmo guarda a menor pontuação das suas 
respectivas ramificações. Caso for o turno do jogador(AI), o algoritmo guarda a 
maior pontuação da suas respectivas ramificações. Este processo repete-se até 
chegar no primeiro *nó* da Árvore. Isso significa que, caso a decisão do jogador 
ganhar ou perder esteja nas mãos do oponente, o Minimax atribui como não sendo 
seguro entrar por este caminho, indicando somente como ‘1’ os caminhos em que o 
jogador possa vencer a partida mesmo que o adversário jogue da melhor forma possível.

Foi aplicado o algoritmo no método,
`private int Minimax(Board board, int depth, int alpha, int beta, bool maximizingPlayer, CancellationToken ct)`
e este é responsável por verificar todas as jogadas possiveis até à profundida 
indicada (`int depth`). Se chegar a profundidade indicada ele retorna um valor 
da função de avaliação estática que reflecte o estado do tabuleiro. Caso existe 
um vencedor, o Minimax retorna o valor máximo de 1000 (AI) ou minímo -1000 
(oponente), em caso de empate retorna o valor de -500 que é entre o 0 e -1000. 


### Função de avaliação estática

Tem como função avaliar o estado do tabuleiro actual, ou seja se o jogador 
estiver perto da vitória têm um maior pontuação e vice-versa.

A implementação da heurística foi efectuada numa classe `G04GLaDOSStaticEvaluation`,
foi efectuada da seguinte forma para a parte da AI:

- Verificar todas as peças da GLaDOS em comparação com todos os corredores de 
vitoria no tablueiro possives.

- Por cada peça no tablueiro é aumentado um ponto, por cada peça consequente 
em linha (horizontal/vertical/diagonal), esse ponto é multiplicado por o 
numero de peças em "linha". Exemplo: 3 peças seguidas =  1x2x3.

- Se for encontrado uma peça do oponente, é retirado a pontuação dessas peças 
em linha.

Após calcular o valor da pontuação da nossa AI é calculada o valor do tablueiro
para o oponente (utilizando a mesma implementação), isto é, quantas peças o
inimigo tem em linha. No final do método é retornado a pontuação da GLaDOS (AI)
subtraido pela a pontuação do oponente.

![CicloFor](./Imgs/Heur.png)


## Referências

*   <a name="ref1">\[1\]</a> Whitaker, R. B. (2016). **The C# Player's Guide**
    (3rd Edition). Starbound Software.

*   <a name="ref2">\[2\]</a> Millington, I. (2019). **AI for Games**
    (3rd Edition). CRC Press.


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