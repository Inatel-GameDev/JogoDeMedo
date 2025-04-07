Vibe jogo de medo medo muito medo tipo lethal company 

Responsabilidades: 
* Dju: Player e Monstros
* Alves: Itens e Mapa 
* Marcelo: Procedural e Online 

# Resumo/Core 

### Resumo do jogo:
 Condenados em uma missão suicida, um grupo de presos deve viajar pelo espaço, coletando recursos e evitando monstros. Para conseguir chegar no final, precisam coletar gasolina, peças e comida, já que a nave inicialmente possui muito pouco. Eles podem pegar em outras naves ou em bases planetárias, sempre podendo escolher parar ou não nesses lugares.
 
 Naves são menores e mais simples, enquanto bases possuem mais recursos porém mais desafiadoras. 
 
 Público alvo: Grupo de amigos que querem sustos e jumpscares enquanto se divertem correndo por aí

### Principais conceitos:
* Coleta de recursos
* Decisão de onde parar 
* Evitar encontros com o monstro, mas da pra matar 
* Trabalho em equipe 
* Comunicação Limitada 

## Direção de Arte e Som
Modelos Low-Poly e com poucos detalhes. 
#### Interfaces
HUD 
Celular 
#### Monstros
#### Ambiente
#### Personagens

## Mecânicas 
#### Personagens 
* Movimentação
* Câmera primeira pessoa
* Movimentação padrão  
    * Corrida limitada com stamina
* Pode comer para recuperar vida 
* Pode pegar ou acionar itens estando dentro do alcance e apertando um botão 
* Possui inventário com espaço limitado 
* Stats diferentes entre players  
    * Vida, velocidade, capacidade, dano de morte 
* Tarefas simples de segurar o botão ou apertar no tempo certo
* Comunicação
    * Chat de proximidade 
* Morrer = explodir 

#### Itens
* Coletáveis padrão: 
    * Combustível, peça de nave, comida, dinheiro 

* Trava zap 
* Mulher casada 
* Espada da cruz de malta 
* Pistola 

#### Mercado 
* Ou na escolha de parar ou não 
* Ou tem que achar 
* Ou já fica dentro da nave 
* Poder comprar arma, armadilha, walkie talkie, comidas, gasolina 

#### Monstros
Cada monstro tem um comportamento específico 
* Um rápido que precisa te achar  
* Enderman da vida 
* Monstro roleta russa   

Russo: 
* Estado Perseguindo 
    * Não possui a localização precisa
      * Recebe a informação da posição a cada 3 segundos
      * Se estiver muito longe, recebe a posição distorcida em alguma direção aleatória
* Estado Perto
    * consegue seguir precisamente 
* Estado Jogando
    * Se encostar em um jogador, abre uma hud para jogarem roleta russa
    * Quem está fora não consegue interagir com eles
    * O jogo é um simples vai e volta até um deles morrer ?

Fungo:
* Estado vagando
    * Percorre o mapa todo com o algoritmo de matriz
    * Solta Poros que ficam presos no chão e parede
        * Poros infectam os jogadores e matam se não forem rápidos o suficiente  
        * Se morrer para o poro, explode em vários outros poros 
    * Infecta automaticamente se enconstar no jogador 

Lerdo: 
* Estado Seguindo 
    * Se move lentamente até o jogador
    * Sabe a sua posição
* Estado atacando 
    * quando chega perto o suficiente dispara um ataque em área 
    * precisa carregar o ataque dando chance para eles fugirem ou atacarem 
    * a cada vez que erra o ataque, ele fica mais rápido e o ataque fica com mais alcance
    * se acertar algum jogador ele volta em um ponto a velocidade
    * Possui uma velocidade máxima


## Ambiente  
#### Mecânica da nave dos jogadores  
* A nave começa com apenas a habilidade de seguir ou parar.
    * Gasta combustivel
* Podem achar peças para melhorar a nave 
    * Scanner da informação sobre as paradas
    * Máquina de melhoria, pode melhorar os atributos do jogador, gasta combustível 
    * Teletransporte ? 

#### Naves/Bases Inimigas 
* Possui Armadilhas
    * De urso e te prende 
    * Bomba ragdoll fudido q n mata
* Para abrir grandes compartimentos de recursos precisa de uma tarefa rápida
    * Segurar
    * Timing 
    * Timing de dupla -> morte -> sai voando 
    * Labirinto ? 
    * Instruções ? 
* Recursos em pouca quantidade espalhados pelo mapa 

## História
#### Porque estão ali 
Apocalipse? Estão presos 
#### O que devem fazer 
Sobreviver o máximo possível?

## Criação de mapas 
Procedural  
Nave 
Base


## Modo Dev 

## Unity 

Objeto jogador: 
* Tag = Player
* RigidBody (Interpolate e Continuous), Capsule Collider, AudioSource
* Scripts: SoundPlayer, Jogador
* Filhos:   
  * Cada Estado é um filho e possui o código e os componentes específicos dele
  * Orientation e Camera Pos ⇾ Navegação e Primeira Pessoa
  * Corpo  ⇾  Mesh Renderer do modelo 
  * Sounds ⇾ Um objeto para armazenar em lista todos os sons disponíveis para o jogador  

Objeto Monstro Base: 
* RigidBody, NavMeshAgent, SoundPlayer,AudioSource
* Filhos:
  * Cada Estado é um filho e possui o código e os componentes específicos dele
  * Corpo  ⇾  Mesh Renderer do modelo
  * Sounds ⇾ Um objeto para armazenar em lista todos os sons disponíveis para o jogador

UML: 
https://lucid.app/lucidchart/467e5d9c-0615-407e-b290-0dc68f0b5770/edit?viewport_loc=-736%2C-836%2C3758%2C1590%2C0_0&invitationId=inv_a7b80d4c-90e6-4854-9ec1-d8450a940b6d