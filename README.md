Dieses Tool soll dabei helfen einfache Graphen live, visuell, animiert darzustellen. 

Zum Herunterladen entweder das gesamte Repository als .zip Archiv herunterladen oder per Git klonen. 
Dann die Graphs.exe Datei ausführen, um das Programm zu starten. 

Das Tool hat aktuell zwei Werkzeuge, die mit der Maus benutzt werden können. Das 'Edit' Tool ist das wichtigere Tool zum Bewegen und Verbinden von Knoten.
Knoten können mit Links-Click per Drag&Drop bewegt werden. Dabei werden der anvisierte Knoten und seine erreichbaren direkten Nachbarn gehighlightet. 
Mit Rechtsclick können Knoten verbunden werden. Erster Rechtsclick zum Markieren des ersten Knoten, Zweiter Rechtclick für eine Kante von A zu B (Schlingen sind nicht erlaubt).

Das zweite implementierte Tool ist nur zum Löschen von Knoten/Kanten gedacht. Ein Linksklick zum markieren und ein Zweiter zum Löschen. Knoten mit inzidenten Kanten löschen ihre Kanten mit.

Alle sonstigen UI Elemente sind mittels Mausklick interagierbar.

Hotkey Mappings :
[C] -> Macht das gleiche was Rechtsklick im Edit Tool macht
