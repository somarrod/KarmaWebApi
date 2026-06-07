# Arquitectura i criteris de disseny del projecte KarmaWebApi

## 1. Visió general

Aquest projecte segueix una arquitectura orientada a domini, amb una separació clara de responsabilitats entre controllers, serveis, models i DTOs.  
L’objectiu principal és aconseguir un codi mantenible, extensible i fidel al domini, evitant concentrar la lògica de negoci en capes incorrectes.

El domini marca les regles; la persistència i el framework s’adapten a ell.

---

## 2. Separació Controller / Service

### 2.1 Controllers

Els controllers són deliberadament fins i no contenen lògica de negoci.

Responsabilitats principals:
- Definir les rutes HTTP (`HttpGet`, `HttpPost`, etc.)
- Gestionar l’autenticació i autorització (`Authorize`)
- Rebre els DTOs d’entrada (arguments d'entrada)
- Delegar la lògica als serveis corresponents

Un controller no pren decisions de negoci ni comprova coherència de dades més enllà de validacions bàsiques de forma.

---

### 2.2 Serveis

Els serveis constitueixen el nucli del sistema i concentren tota la lògica de negoci.

Responsabilitats:
- Aplicar les regles del domini
- Validar coherència entre entitats
- Coordinar múltiples entitats si és necessari
- Garantir la integritat de les operacions

Exemples de lògica ubicada en serveis:
- Validació del camp `Editable` en Categoria
- Comprovació que una data cau dins d’una Avaluació
- Actualització del karma d’alumne i grup
- Prohibició d’editar o eliminar puntuacions

Aquesta separació permet reutilització, testabilitat i una evolució més segura del sistema.

---

## 3. Models

Els models reflecteixen estrictament el model orientat a objectes de Karma que ha guiat aquest disseny.

Principis seguits:
- Tot atribut del model orientat a objectes existeix en el model
- Relacions obligatòries es representen explícitament
- Les dades històriques es conserven mitjançant snapshots

---

## 4. Claus primàries 

Totes les claus primàries utilitzades son claus simples. 
Habitualment es tracta de camps de tipus long. 
Excepte en:
- Profesor que utilitza el seu codi de professor assignat per la GVA
- Alumne que utilitza el NIA
- AnyEscolar que utilitza un int, ja que composa el seu id utilitzant l'anyescolar: exemple 2526 o 2627.

---

## 5. Ús de DTOs

Els DTOs defineixen el contracte de l’API i separen el domini de les dades d’entrada.

Criteris aplicats:
- Els DTOs no són models
- Només contenen els camps que l’usuari pot indicar
- No inclouen informació calculada ni de domini intern

### Ús dels identificadors en DTOs

Regla aplicada:
- Si l’ID identifica l’objecte a modificar → s’inclou al DTO
- Si l’ID el genera el sistema → no s’inclou

Exemples:
- `CategoriaCrearDTO` no inclou `IdCategoria`
- `CategoriaEditarDTO` sí inclou `IdCategoria`

---

## 6. Tipus de dades i convencions

### 6.1 Identificadors

- Tots els identificadors del domini són de tipus `long`
- Coherència total entre model, servei, controller i base de dades
- S’eviten conversions innecessàries

En rutes HTTP es recomana utilitzar restriccions explícites: {id:long}

---

### 6.2 Ús de string

S’ha normalitzat l’ús de:
  string i no String

Motiu:
- Ús idiomàtic de C#
- Millor llegibilitat
- Coherència amb les bones pràctiques modernes

---

## 7. Validacions de negoci clau

Totes les validacions de negoci es realitzen en serveis, mai en controllers.

### 7.1 Categoria editable

- Si `Editable = false`, el nombre de punts no pot modificar-se
- El valor indicat s’ha de correspondre exactament amb el definit en la categoria
- El servei valida i/o força el valor

Això permet mostrar el valor a l’usuari sense permetre alterar-lo.

---

### 7.2 Puntuació

- No es pot editar
- No es pot eliminar
- Rectificar implica crear una nova puntuació inversa o correctora

Això garanteix traçabilitat i integritat històrica.

---

### 7.3 Validació de dates

- Tota puntuació ha de pertànyer a una avaluació concreta
- La data de l’esdeveniment ha de caure dins del rang de dates de l’avaluació
- La validació es realitza en el servei

---

## 8. Snapshots de dades

Algunes entitats emmagatzemen dades “instantànies”:
- Identificador i nom de la classe
- Identificador i nom del grup

Objectiu:
- Conservar el context original encara que les dades canvien amb el temps
- Permetre auditories i informes coherents

---

## 9. Relacions i OnModelCreating

`OnModelCreating` s’utilitza exclusivament quan EF Core no pot deduir correctament el model.

S’empra per:
- Definir claus compostes
- Definir claus foranes compostes
- Controlar comportaments d’eliminació (`DeleteBehavior`)
- Evitar cascades no desitjades

No s’utilitza per duplicar configuracions que ja poden deduir-se per convenció.

---

## 10. Migracions i base de dades

Criteris seguits:
- La migració inicial es crea quan el model està estabilitzat
- Durant la fase de disseny és acceptable reinicialitzar migracions
- En fases posteriors, les migracions es consideren historial immutable

Una migració inicial amb mètodes `Up()` i `Down()` buits indica un problema de desincronització entre el model i el snapshot.

---

## 12. Postman

El projecte disponsa d'una col·lecció Postman amb exemples reals de les dades a utilitzar.

