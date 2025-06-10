# POS - Projekt in C#

## Allgemeine Infos
Projektname: Antonio Beats
Projektgruppe: Reichart Tobias, Bitschnau Chiara
Klasse: 2AHIF
Jahr: 2025

Betreuer: Diem Lukas, Bechtold David                                                                                
Kurzbeschreibung: 
Programm zum Abspielen von Musik. Dort können Playlists erstellt und gespeichert werden.
Auch können Songs bzw Audios in das Programm hochgeladen werden


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




## Bedienungsanleitung
- App öffnen -> Playlists werden direkt angezeigt -> Doppelklick auf eine Playlist um sie genauer anzuschauen -> über Buttons Songs / Playlists hinzufügen / umbennen usw.

## Bekannte Bugs / Probleme die noch vorliegen
## Erweiterungsmöglichkeiten

