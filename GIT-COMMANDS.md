# GIT COMMANDS

## Setup

| Befehl                                         | Beschreibung                                   |
| ---------------------------------------------- | ---------------------------------------------- |
| `git config --global user.name "full name"`    | Erfoderlich, da Name in Git History auftaucht  |
| `git config --global user.email "valid email"` | Erfoderlich, da sie mit Verlauf verknüpft wird |
| `git config --global color.ui auto`            | Farben in Befehlszeile verwenden               |

## Setup & Init

| Befehl            | Beschreibung                                   |
| ----------------- | ---------------------------------------------- |
| `git init`        | Neues Repository anlegen                       |
| `git clone [url]` | kopieren von einem Online-Repository am Anfang |

## Stage & Snapshot

| Befehl                                 | Beschreibung                                                     |
| -------------------------------------- | ---------------------------------------------------------------- |
| `git status`                           | Änderungen im Arbeitsverzeichnis anzeigen lassen                 |
| `git add .`                            | alle Änderungen für ein Commit vorschlagen                       |
| `git commit -m "Lorum ipsum"`          | Änderungen in das lokale Repo commiten / eintragen               |
| `git commit -am "Lorum ipsum"`         | Add und Commit zusammen ausführen                                |
| `git commit --amend -m "Lorum ipsum"`  | Letzte Message ändern (**nur bei lokalen Commits empfohlen!**)   |
| `git commit --amend --no-edit`         | Änderungen zum letzten Commit hinzufügen und Message beibehalten |
| `git log`                              | Änderungsverlauf anzeigen                                        |
| `git log --graph --oneline --decorate` | Änderungsverlauf als knappe Aufschlüsselung darstellen           |

| Befehl           | Beschreibung                          |
| ---------------- | ------------------------------------- |
| `git stash`      | Änderungen zwischenspeichern          |
| `git stash list` | Stash auflisten                       |
| `git stash pop`  | Änderungen wiederherstellen           |
| `git stash drop` | Letzte Änderungen vom Stash verwerfen |

## Share & Update

| Befehl                         | Beschreibung                                                       |
| ------------------------------ | ------------------------------------------------------------------ |
| `git remote add [alias] [url]` | Git URL als alias hinzufügen                                       |
| `git fetch [alias]`            | Informationen aller Branchen von Remote synchronisieren            |
| `git merge [alias]/[branch]`   | Änderungen von Remote zusammenführen (kann **Konflikte** auslösen) |
| `git push [alias] [branch]`    | eigene lokale Repo-Version hochladen                               |
| `git pull`                     | fetch und merge vom aktuellen remote branch                        |

## Aufräumen

| Befehl          | Beschreibung                                                              |
| --------------- | ------------------------------------------------------------------------- |
| `git clean -df` | Ohne Nachfrage unversionierte Dateien löschen (praktisch für Builds etc.) |

[git homepage](https://git-scm.com)
