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

    //public float minimumInitialScale;
    private Vector3 posInit;
    private Vector3 posCursor;

    // marque quand le bouton de la souris est enfonce
    bool enCours = false;

    public Tilemap tilemap;
    GameObject pheromone;
    GridLayout gridLayout;


    void Start()
    {
        gridLayout = transform.parent.GetComponentInParent<GridLayout>();
    }


    void Update()
    {
        
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
            pheromone.transform.Find("Pheromone_Range").transform.localScale = dist * new Vector3(1f, 1f, 1f);

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

    //cette fonction sert à trouver la case accessible la plus proche pour une fourmi quand elle est trigger par une pheromone afin de s'y rendre7
    //il suffit de convertir la postion globale de la pheromone en position dans la tilemap
    //ensuite si on a une coordonnée du type (x.5 , y , z) -> on regarde la tile du haut ou la tile du bas
    //Si on a une coordonnée de type (x, y.5 , z) -> on regarde à gauche ou à droite de la tile
    //une des tiles sera accessible et donc la direction de la fourmi
    //l'autre sera une tile pleine et donc celle à creuser dès que la fourmi à atteint sa destination
    //si les deux tiles sont pleines on annule la pheromone

    public Vector3 findNearestAccesibleTile(Vector3 pheroWorldPosition)
    {
        //Debug.Log("je suis dans la fonction findNearestAccesibleTile");
        Debug.Log("pheroWorldPosition : " + pheroWorldPosition);
        Debug.Log("cellPosition = " + gridLayout.WorldToCell(pheroWorldPosition));

        (string direction, Vector3 ptInteret) = giveInfo(pheroWorldPosition);
        Debug.Log("direction  = "+ direction + " ,ptInteret = " + ptInteret);
        //Vector3Int cellposition = gridLayout.WorldToCell(pheroWorldPosition);
        //Debug.Log("Pheropositon (in cell coordinates) ="+cellposition);
        if (direction == "bas")
        {

        }
        //Vector3 destposition = gridLayout.CellToWorld(cellposition);
        Vector3 destposition = new Vector3(0, 0, 0);

        return destposition;
        
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

    /*
   
    Pour une position du monde donné, retourne ou est la "position d'intérêt" la plus proche, cad sur le centre d'une case ou d'un rebord, 
    ainsi que la direction, cad si ce point d'intéret est sur le haut, le bas, le centre ou les côtés d'une case.
    */
    private (string direction, Vector3 ptInteret) giveInfo(Vector3 worldpos) {

        Vector3Int cellPosition = gridLayout.WorldToCell(worldpos);
        Vector3 ptInteret;
        Vector3 posCell;
        string direction;


        // les differents points d'interet sur une case : centre haut bas gauche droite
        posCell = gridLayout.CellToWorld(cellPosition);
        Vector3 centre = posCell;
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

        if (worldpos.x < posCell.x + 4.8f && worldpos.x > posCell.x + 1.6f && worldpos.y < posCell.y + 4.8f && worldpos.y > posCell.y + 1.6f) // centre
        {
            ptInteret = centre + new Vector3(0, 0, -1);
            direction = "centre";
        }
        else
        {
            if (worldpos.y - posCell.y > worldpos.x - posCell.x)                                // partie haut gauche
            {
                if (worldpos.y - posCell.y < 6.4f - worldpos.x + posCell.x)                      // triangle gauche
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
                if (worldpos.y - posCell.y < 6.4f - worldpos.x + posCell.x)                     // bas
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

        return (direction, ptInteret);
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


}
