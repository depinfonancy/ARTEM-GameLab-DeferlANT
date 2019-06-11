using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Mouse_control : MonoBehaviour
{
    // prise en compte de TOUTES les tiles
    public TileBase plein;

    public TileBase salle;
    public TileBase salled;
    public TileBase salleg;

    public TileBase virhd;
    public TileBase virhg;
    public TileBase virbd;
    public TileBase virbg;
    public TileBase vertical;
    public TileBase horizontal;

    public TileBase creuseh;
    public TileBase creuseb;
    public TileBase creused;
    public TileBase creuseg;

    public TileBase Tg;
    public TileBase Td;
    public TileBase Th;
    public TileBase Tb;

    public TileBase intersection;

    public GameObject Pheromone;

    // recuperer le script qu'on va appeler
    //public GameObject obstacle_init;

    //public float minimumInitialScale;
    private Vector3 posInit;
    private Vector3 posCursor;

    // marque quand le bouton de la souris est enfonce
    bool enCours = false;

    public Tilemap tilemap;
    GameObject pheromone;

    // tableau des tiles indexees en fonction de leurs ouvertures (haut : 1er bit ; droite : 2eme bit...)
    TileBase[] liste_des_tiles;

    private void Start()
    {
        liste_des_tiles = new TileBase[] { plein, creuseh, creused, virhd, creuseb, vertical, virbd, Tg, creuseg, virhg, horizontal, Tb, virbg, Td, Th, intersection, salle, salled, salleg};
    }

    void Update()
    {
        GridLayout gridLayout = transform.parent.GetComponentInParent<GridLayout>();
        Vector3 posFin = Camera.main.ScreenToWorldPoint(Input.mousePosition);


        // controle d'un petit curseur qui montre ou sera placee la prochaine pheromone (on limite les possibilites 
        // de placement pour essayer de rendre le code plus facile).
        Vector3Int cellPosition = gridLayout.WorldToCell(posFin);
        Vector3 ptInteret;


        // les differents points d'interet sur une case : centre haut bas gauche droite
        posCursor = gridLayout.CellToWorld(cellPosition);
        Vector3 centre = posCursor;
        centre.x += 3.2f;
        centre.y += 3.2f;
        Vector3 haut = centre;
        haut.y += 3.2f;
        Vector3 droite = centre;
        droite.x += 3.2f;
        Vector3 bas = centre;
        bas.y -= 3.2f;
        Vector3 gauche = centre;
        gauche.x -= 3.2f;
        string direction;


        // equations pour retrouver dans quelle portion de case la souris se trouve
        if (posFin.x < posCursor.x + 4.8f && posFin.x > posCursor.x + 1.6f && posFin.y < posCursor.y + 4.8f && posFin.y > posCursor.y + 1.6f) // centre
        {
            ptInteret = centre + new Vector3(0, 0, -1);
            direction = "centre";
        }
        else
        {
            if (posFin.y - posCursor.y > posFin.x - posCursor.x)                                // partie haut gauche
            {
                if (posFin.y - posCursor.y < 6.4f - posFin.x + posCursor.x)                      // triangle gauche
                {
                    ptInteret = gauche + new Vector3(0, 0, -1);
                    direction = "gauche";
                }
                else                                                                            // haut
                {
                    ptInteret = haut + new Vector3(0, 0, -1);
                    direction = "haut";
                }
            }
            else                                                                                // bas droite
            {
                if (posFin.y - posCursor.y < 6.4f - posFin.x + posCursor.x)                     // bas
                {
                    ptInteret = bas + new Vector3(0, 0, -1);
                    direction = "bas";
                }
                else                                                                            // droite
                {
                    ptInteret = droite + new Vector3(0, 0, -1);
                    direction = "droite";
                }
            }
        }

        // on va pouvoir maintenant utiliser le ptInteret pour poser nos animations (pheromones et tout)
        GetComponent<Transform>().position = ptInteret + new Vector3(0, 0, -1);


        // paint une nouvelle case
        if (Input.GetMouseButtonDown(1))
        {
            creercase(direction, cellPosition);
            // on recupere les tiles autour de celle cliquee pour les actualiser ensuite. Elles sont dans un ordre bien precis (haut droite bas gauche)
            TileBase tile1 = tilemap.GetTile(cellPosition);
            TileBase tileh = tilemap.GetTile(cellPosition + new Vector3Int(0, 1, 0));
            TileBase tiled = tilemap.GetTile(cellPosition + new Vector3Int(1, 0, 0));
            TileBase tileb = tilemap.GetTile(cellPosition + new Vector3Int(0, -1, 0));
            TileBase tileg = tilemap.GetTile(cellPosition + new Vector3Int(-1, 0, 0));
            // on recupere dans le tableau des tiles les index de tile pour chaque tile
            int[] tiles_a_changer = position_dans_tableau(liste_des_tiles, tile1, tileh, tiled, tileb, tileg);
            GetComponent<Obstacle_init>().actualise_brush(cellPosition, tiles_a_changer);

        }


        // a l'appui sur le bouton gauche de la souris un cercle de pheromone apparait
        if (Input.GetMouseButtonDown(0))
        {
            enCours = true;
            SpawnPheromone(ptInteret);
        }


        // la pheromone en cours devient independante de la souris
        if (Input.GetMouseButtonUp(0))
        {
            enCours = false;
            pheromone = null;
        }


        // update de la taille de la pheromone avec la souris
        if (enCours)
        {
            //Vector3 posFin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float dist = Vector3.Distance(posFin, posInit);
            pheromone.transform.Find("Pheromone_Range").transform.localScale = dist * new Vector3(0.1f, 0.1f, 0.1f);

            //Ici il faut modifier le collider pour qu'il match avec le sprite, il faut trouver le bon rapport ou la bonne méthode
            /*
            CircleCollider2D collider = pheromone.transform.Find("Pheromone_Range").GetComponent<CircleCollider2D>();
            collider.radius = dist*0.1f;
            */
        }

    }



    // faire apparaitre un cercle de pheromones a la pos cliquee de la souris
    void SpawnPheromone(Vector3 origin)
    {
        posInit = origin;
        posInit.z = 0;
        pheromone = Instantiate(Pheromone, origin, Quaternion.identity);
        pheromone.SetActive(true);
    }


    // fonction de base de creusage
    void creercase(string direction, Vector3Int cellposition)
    {
        TileBase travail = tilemap.GetTile(cellposition);


        if (direction == "centre")  // creuser une salle dans un endroit ou on peut en faire une
        {
            if (travail == horizontal)
            {
                tilemap.SetTile(cellposition, salle);
            }
            else
            {
                if (travail == creused)
                {
                    tilemap.SetTile(cellposition, salled);
                }
                else
                {
                    if (travail == creuseg)
                    {
                        tilemap.SetTile(cellposition, salleg);
                    }
                }
            }
        }
        else
        {
            if (possible_de_creuser(direction, cellposition, travail))
            {
                if (direction == "haut")
                {
                    miseAJourCase(cellposition, direction);
                    miseAJourCase(cellposition + new Vector3Int(0, 1, 0), "bas");
                }
                else
                {
                    if (direction == "bas")
                    {
                        miseAJourCase(cellposition, direction);
                        miseAJourCase(cellposition + new Vector3Int(0, -1, 0), "haut");
                    }
                    else
                    {
                        if (direction == "gauche")
                        {
                            miseAJourCase(cellposition, direction);
                            miseAJourCase(cellposition + new Vector3Int(-1, 0, 0), "droite");
                        }
                        else
                        {
                            miseAJourCase(cellposition, direction);
                            miseAJourCase(cellposition + new Vector3Int(1, 0, 0), "gauche");
                        }
                    }
                }
            }
        }
    }

    // effectuer un test avant de creuser pour gerer certaines collisions potentielles (les salles creusees et pas creuser dans le plein)
    bool possible_de_creuser(string direction, Vector3Int cellposition, TileBase travail)
    {
        // cases au-dessus et en-dessous
        TileBase caseh = tilemap.GetTile(cellposition + new Vector3Int(0, 1, 0));
        TileBase caseb = tilemap.GetTile(cellposition + new Vector3Int(0, -1, 0));

        if ((tilemap.GetTile(cellposition) == salle) || (tilemap.GetTile(cellposition) == salled) || (tilemap.GetTile(cellposition) == salleg))
        {
            if (direction == "haut" || direction == "bas")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            if (direction == "haut" && (caseh == salle || caseh == salled || caseh == salleg || (travail == plein && caseh == plein)))
            {
                return false;
            }
            else
            {
                if (direction == "bas" && (caseb == salle || caseb == salled || caseb == salleg || (travail == plein && caseb == plein)))
                {
                    return false;
                }
                else
                {
                    if (direction == "droite" && travail == plein && tilemap.GetTile(cellposition + new Vector3Int(1, 0, 0)) == plein)
                    {
                        return false;
                    }
                    else
                    {
                        if (direction == "gauche" && travail == plein && tilemap.GetTile(cellposition + new Vector3Int(-1, 0, 0)) == plein)
                        {
                            return false;
                        }
                        else { return true; }
                    }
                }
            }
        }
    }



    // update en case pas case
    void miseAJourCase(Vector3Int cellPosition, string direction)
    {
        TileBase travail = tilemap.GetTile(cellPosition);
        if (travail == plein)
        {
            if (direction == "haut")
            {
                tilemap.SetTile(cellPosition, creuseh);
            }
            else
            {
                if (direction == "bas")
                {
                    tilemap.SetTile(cellPosition, creuseb);
                }
                else
                {
                    if (direction == "gauche")
                    {
                        tilemap.SetTile(cellPosition, creuseg);
                    }
                    else
                    {
                        tilemap.SetTile(cellPosition, creused);
                    }
                }
            }
        }
        else
        {
            if (travail == creuseh)
            {

                if (direction == "bas")
                {
                    tilemap.SetTile(cellPosition, vertical);
                }
                else
                {
                    if (direction == "gauche")
                    {
                        tilemap.SetTile(cellPosition, virhg);
                    }
                    else
                    {
                        tilemap.SetTile(cellPosition, virhd);
                    }
                }

            }
            else
            {
                if (travail == creuseb)
                {
                    if (direction == "haut")
                    {
                        tilemap.SetTile(cellPosition, vertical);
                    }
                    else
                    {

                        if (direction == "gauche")
                        {
                            tilemap.SetTile(cellPosition, virbg);
                        }
                        else
                        {
                            tilemap.SetTile(cellPosition, virbd);
                        }

                    }
                }
                else
                {
                    if (travail == creused)
                    {
                        if (direction == "haut")
                        {
                            tilemap.SetTile(cellPosition, virhd);
                        }
                        else
                        {
                            if (direction == "bas")
                            {
                                tilemap.SetTile(cellPosition, virbd);
                            }
                            else
                            {
                                if (direction == "gauche")
                                {
                                    tilemap.SetTile(cellPosition, horizontal);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (travail == creuseg)
                        {
                            if (direction == "haut")
                            {
                                tilemap.SetTile(cellPosition, virhg);
                            }
                            else
                            {
                                if (direction == "bas")
                                {
                                    tilemap.SetTile(cellPosition, virbg);
                                }
                                else
                                {

                                    tilemap.SetTile(cellPosition, horizontal);

                                }
                            }
                        }
                        else
                        {
                            if (travail == virbd)
                            {
                                if (direction == "haut")
                                {
                                    tilemap.SetTile(cellPosition, Tg);
                                }
                                else
                                {

                                    if (direction == "gauche")
                                    {
                                        tilemap.SetTile(cellPosition, Th);
                                    }

                                }
                            }
                            else
                            {
                                if (travail == virbg)
                                {
                                    if (direction == "haut")
                                    {
                                        tilemap.SetTile(cellPosition, Td);
                                    }
                                    else
                                    {


                                        tilemap.SetTile(cellPosition, Th);


                                    }
                                }
                                else
                                {
                                    if (travail == virhd)
                                    {

                                        if (direction == "bas")
                                        {
                                            tilemap.SetTile(cellPosition, Tg);
                                        }
                                        else
                                        {
                                            if (direction == "gauche")
                                            {
                                                tilemap.SetTile(cellPosition, Tb);
                                            }
                                        }

                                    }
                                    else
                                    {
                                        if (travail == virhg)
                                        {

                                            if (direction == "bas")
                                            {
                                                tilemap.SetTile(cellPosition, Td);
                                            }
                                            else
                                            {

                                                tilemap.SetTile(cellPosition, Tb);

                                            }

                                        }
                                        else
                                        {
                                            if (travail == horizontal)
                                            {
                                                if (direction == "haut")
                                                {
                                                    tilemap.SetTile(cellPosition, Tb);
                                                }
                                                else
                                                {
                                                    if (direction == "bas")
                                                    {
                                                        tilemap.SetTile(cellPosition, Th);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (travail == vertical)
                                                {


                                                    if (direction == "gauche")
                                                    {
                                                        tilemap.SetTile(cellPosition, Td);
                                                    }
                                                    else
                                                    {
                                                        tilemap.SetTile(cellPosition, Tg);
                                                    }


                                                }
                                                else
                                                {
                                                    if (travail == Tb)
                                                    {
                                                        tilemap.SetTile(cellPosition, intersection);
                                                    }
                                                    else
                                                    {
                                                        if (travail == Td)
                                                        {
                                                            tilemap.SetTile(cellPosition, intersection);
                                                        }
                                                        else
                                                        {
                                                            if (travail == Th)
                                                            {
                                                                tilemap.SetTile(cellPosition, intersection);
                                                            }
                                                            else
                                                            {
                                                                if (travail == Tg)
                                                                {
                                                                    tilemap.SetTile(cellPosition, intersection);
                                                                }
                                                                else
                                                                {
                                                                    if (travail == salled)
                                                                    {
                                                                        tilemap.SetTile(cellPosition, salle);
                                                                    }
                                                                    else
                                                                    {
                                                                        if (travail == salleg)
                                                                        {
                                                                            tilemap.SetTile(cellPosition, salle);
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }


    // parcours du tableau pour avoir l'index dans le tableau
    int[] position_dans_tableau(TileBase[] tableau, TileBase obj1, TileBase obj2, TileBase obj3, TileBase obj4, TileBase obj5)
    {
        int[] i = { 0, 0, 0, 0, 0 };
        for (int j = 0; j < 19; j++)
        {
            if (tableau[j] == obj1)
            {
                i[0] = j;
            }
            if (tableau[j] == obj2)
            {
                i[1] = j;
            }
            if (tableau[j] == obj3)
            {
                i[2] = j;
            }
            if (tableau[j] == obj4)
            {
                i[3] = j;
            }
            if (tableau[j] == obj5)
            {
                i[4] = j;
            }
        }
        Debug.Log(i[0]);
        return i;
    }

}
