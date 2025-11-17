# TP2 – Ouverture et sauvegarde de fichiers OFF

> Modélisation géométrique – Unity 6000.0.57f1  
> Le projet charge, centre, normalise et exporte des maillages au format `.off`.

Le projet utilise trois scripts :
- **TextFileReader.cs** : lecture du fichier OFF  
- **MeshWriter.cs** : chargement, centrage, normalisation, construction du mesh  
- **MeshExporter.cs** : export d’un nouveau mesh au format OFF  

Une seule scène contient la mise en place : un MeshFilter, un MeshRenderer et ces scripts.

---

## Format OFF

Un fichier OFF contient :
- Une ligne `OFF`
- Une ligne `<nbVertices> <nbFaces> 0`
- Les positions des vertices
- Les faces triangulaires sous la forme `3 i j k`

Exemple :
```
OFF
8 12 0
0.256 0.365 0.569
...
3 0 1 2
3 2 5 9
```

---

## Exercice 1 – Chargement du maillage

Le script **TextFileReader** lit le fichier via `File.ReadAllLines` et stocke chaque ligne dans une liste.

Dans **MeshWriter**, le chargement consiste à boucler dans cette liste afin de lire chaque ligne une à une :

La boucle :
- Lis `nbVertices` et `nbTriangles` sur la deuxième ligne  
- Lis les vertices  
- Lis les indices des triangles  

Les données sont mises dans des `List<Vector3>` et `List<int>` puis envoyées au Mesh Unity.

/!\ Le fichier choisit pour la lecture correspond à la variable File (interpréteur Unity)


0 = Plan / 1 = Cube / 2 = Lapin / 3 = Buddha


Le code charge le mesh uniquement lorsque la variable change, par soucis de performance.

---

## Exercice 2 – Centre de gravité

Le centre de gravité est la moyenne de tous les vertices.  

```csharp
// Vec3 gravityCenter = somme des vertices
gravityCenter /= nbVertices;
for (int i = 0; i < vertices.Count; i++)
{
    vertices[i] = vertices[i] - gravityCenter;
}
```

Chaque vertex est ensuite recentré :

```csharp
vertices[i] = vertices[i] - gravityCenter;
```

---

## Exercice 3 – Normalisation

On cherche la plus grande valeur absolue parmi tous les vertices.  
Chaque vertex est divisé par cette valeur :

```csharp
vertices[i] /= maximum;
```

Le modèle tient alors dans un cube `[-1, 1]`.

---

## Exercice 4 – Calcul des normales

La fonction `GetNormal(A, B, C)` utilise un produit vectoriel :

```csharp
return Vector3.Cross(AB, AC);
```

`mesh.RecalculateNormals()` est utilisé pour l’affichage final.

---

## Exercice 5 – Export du maillage

**MeshExporter** recrée un fichier OFF :

1. `OFF`
2. `<nbVertices> <nbFaces> 0`
3. Positions des vertices
4. Index des triangles sous la forme `3 i j k`

Le bouton `ExportMeshButton` sauvegarde le fichier dans :

```
Assets/<fileName>
```

Le fichier est compatible MeshLab.
