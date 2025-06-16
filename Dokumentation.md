# POS - Projekt in C#

## Allgemeine Infos
Projektname: delos

Projektgruppe: Reichart Tobias, Bitschnau Chiara

Klasse: 2AHIF

Jahr: 2025

Betreuer: Diem Lukas, Bechtold David      

Kurzbeschreibung:
delos ist ein Programm zum Abspielen von Musik, das verschiedene Funktionen rund um Playlists und Audiodateien bietet. Nutzerinnen und Nutzer können eigene Playlists erstellen, verwalten und speichern, um ihre Lieblingssongs zu sortieren und jederzeit wieder abzuspielen. Zusätzlich besteht die Möglichkeit, eigene Songs oder Audiodateien in das Programm hochzuladen. Diese werden dann gespeichert und können später in Playlists eingefügt werden

Screenshots aus dem Programm: 

![image](https://github.com/user-attachments/assets/5e1493b0-1fcf-4724-bba6-1cba7de191d5)
![image](https://github.com/user-attachments/assets/85bd924d-85cf-4243-8616-a35040392495)
![image](https://github.com/user-attachments/assets/bf844b99-8a23-43d3-8927-a578b4df0325)
![image](https://github.com/user-attachments/assets/0cb28ca0-f12d-4d2d-972a-27e405d62882)

## Must-Haves
- Musik abspielen (Pause, Play, Skip, Vorspülen)
- Erstellen und Erweitern von Playlists
- Importieren von Songs
- Favoriten
- Shuffle
- Volume-Slider
- Playlist-Klasse mit allgemeinen Infos wie Songanzahl, Dauer der Playlist, ...
- Song Klasse mit Infos wie Ersteller, Dauer, ...


## Nice-To-Haves
- Suchoption
- Autoplay
- Loop
- Songs sotriteren bzw. Ordnung änderbar (Date, Artist, Name)
- Webserver mit Songs
- Wiedergabezeitpunkt speichern
- Bild zum Song
- Warteschlange

## Projektzeitplan:
| Wann? | Was? | Wer? | Fortschritt |
|----------|----------|----------|----------|
| 14.05 | Grunddesign der GUI | Chiara | 100% |
| 14.05 | Grundaufbau der Klassen & Überarbeitung der Planung | Tobias | 100% |
| 21.05 | Verbessern des Designs | Chiara | 100% |
| 28.05 | User Control für Play Pause usw. | Chiara | 100% |
| 27.05 + 28.05 | Klassen so gut wie fertig coden | Tobias | 100% |
| 04.06 | (erfolglose) Fehlerbehebung | Chiara, Tobi, Herr Bechtold | 100% |
| 06.06 | (erfolgreiche) Fehlerbehebung | Chiara, Tobi, Herr Diem | 100% |
| 10.06 | Fehler suchen und behandeln| Chiara, Tobi | 100% |
| 06.06 | (erfolgreiche) Fehlerbehebung | Chiara, Tobi, Herr Diem | 100% |
| 15.06 | Keyboardshortcuts | Chiara | 100% |



## Klassen
### Song Klasse
- +Name:string
- +Length:int
- +ReleaseDate:DateTime
- +Artist:string
- +Progress:int
- +Album:string
- --------------------------
- +Song(path:string)
- --------------------------
- +loadFromMetaData():void
- +editMetaData():void
- +override ToString():string
- +serializeToString():string
- +deserialize():Song
- --------------------------

### Playlist Klasse
- +Songanzahl:int 
- +Shuffle:bool 
- +Repeat:bool 
- +Name:string 
- +Playtime:int 
- +SongList[Song]:List 
- +CurrentSong:int 
- +SongListSorted[Song]:List
- --------------------------
- +Playlist(name:string)
- --------------------------
- +import(path:string):void 
- +save(path:string):void 
- +addSong(Song):void 
- +removeSong(Song):void 
- +skip():void 
- +sort():void 
- +nextSong():void 
- +serialize()

## Wie arbeiten die Klassen miteinander?
- Playlistklasse -> um Playlisten zu erstellen und bearbeiten
- Songklasse -> um Songs zu "erstellen" und bearbeiten
- Playerklasse -> um die Musik abzuspielen

## Bedienungsanleitung / genauere Anleitung
Wenn man die App öffnet, sieht man direkt alle Playlists, die man schon erstellt hat. Diese werden automatisch angezeigt, ohne dass man erst irgendwo draufklicken muss. Man kann dann durch die Liste scrollen und sich anschauen, welche Playlists es gibt und kann über Buttons die Playlist bearbeiten oder Details verändern. Wenn man eine Playlist genauer anschauen will, macht man einfach einen Doppelklick darauf. Dann öffnet sich die Playlist und man sieht alle Songs, die darin gespeichert sind. In diesem Bereich gibt es auch verschiedene Buttons, mit denen man etwas an der Playlist oder den Songs verändern kann. Zum Beispiel kann man neue Songs hinzufügen oder den Namen einer bestehenden Playlist ändern.

## Bekannte Bugs / Probleme die noch vorliegen
## Erweiterungsmöglichkeiten / Zukunfsaussichten
Ideen zur Erweiterung: 
- Optionen um die Playlist zu teilen oder geteilte Playlisten importieren
- einen Webserver benutzen, um die Songs über das Internet zu empfangen / abzuspielen
- die Metadaten über einen Click eines Buttons direkt zu ändern um in Zukunft die Songs so abzuspielen, wie sie geändert wurden

