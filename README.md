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

#### Mercado 
* Ou na escolha de parar ou não 
* Ou tem que achar 
* Ou já fica dentro da nave 
* Poder comprar arma, armadilha, walkie talkie, comidas, gasolina 

#### Monstros
Cada monstro tem um comportamento específico 
* Um rápido que precisa te achar 
* Um lerdo que sempre sabe sua posição (lesma imortal - menina lethal company) 
* Enderman da vida 
* Monstro roleta russa  

Movimentação 	
Ataque 
Habilidades
Estados

## Ambiente  
#### Mecânica da nave dos jogadores  
* A nave começa com apenas a habilidade de seguir em frente e parar.
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

## Código 
Classes principais: 
* Jogador (máquina de estado)
* Monstro (máquina de estado)
* Estado 
* GameManager

## Unity 

Objeto jogador: 
* RigidBody, Collider 
* Filhos cada um com um estado 