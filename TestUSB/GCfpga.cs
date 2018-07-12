using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Windows.Forms;

namespace GeCoSwell
{

    //-----------------------------------
    //Class pour mémoriser les adresses et les noms correspondant
    //-----------------------------------
    public class AdresseFPGA
    {
        public string Adresse { get; set; }
        public string Nom { get; set; }
        public string Use { get; set; }
        public string Exp { get; set; }
        // est utile au double pulse BO = boucle ouverte 
        //DP = double pulse
        //OP = méthode d'opposition
        //all => tout les cas
        public bool Bin { get; set; } // si true valeur en binaire sinon en deci
        public double Pas { get; set; } // le pas de la valeur en général 1 ou 0,1

        public AdresseFPGA() //initialise des valeurs de base
        {
            Pas = 1;
            Bin = false;
        }
    }

    class GCfpga
    {
        static List<AdresseFPGA> listadresse; //initialisation de la list des adresse
        //static string message_erreur; // message erreur qui correspont à l'explication du dernier envoi


        //----------------------------------------------------------------------
        //Fonction qui transforme de décimal vers binaire ou de binaire vers décimals.
        //
        //valeur la valeur à convertir en 10bits
        //vers10bits 
        //1=> vers 10bits ; 
        //0=> vers 1023 ;
        //-1 renvoie tel quel ;
        //pas => pas du signal pour transformer correctement en signal
        //----------------------------------------------------------------------
        public static string Conv_1023_vers_10bits(string valeur, int vers10bits, double pas)
        {
            string retour = "";
                        
            if (vers10bits == 1) // si la conversion est vers un 10bits
            {
                valeur = (double.Parse(valeur) / pas).ToString(); //pour récupérer une valeur entière
                for (int i = 0; i < 10; i++)
                {
                    if (int.Parse(valeur) - (Math.Pow(2, 9 - i)) >= 0) // si notre valeur est plus grande que 2 pui(9 - i)
                    {
                        retour = retour + "1"; // rajoute un 1
                        valeur = (int.Parse(valeur) - (Math.Pow(2, 9 - i))).ToString();
                    }
                    else
                    {
                        retour = retour + "0"; // rajoute un 0
                    }
                }
                return retour;

            }
            else if (vers10bits == 0) // Si on a une valeur 10bits que l'on veux convertir en numérique
            {
                int i = 0;
                retour = "0";
                foreach (char c in valeur)
                {
                    if (c == '1')
                    {
                        retour = (int.Parse(retour) + Math.Pow(2, 9 - i)).ToString();
                    }
                    i++;
                }
                return (double.Parse(retour) * pas).ToString();
            }
            return valeur;

        }

        //----------------------------------------------------------------------
        //Fonction qui écrit le mot de 16bits et qui l'envoie
        //
        //nom => nom de l'objet envoyer ce qui permet d'avoir l'adresse sur le fpga
        //value => valeur à envoyer
        //type => 
        //0=> lecture signal 10bits vers 10bits
        //1=> écriture signal 1023 vers 10bits
        //pas => pas de la valeur pour la transformer après en la bonne valeur
        //----------------------------------------------------------------------
        public static string Send_Param(string nom, string value, int type, double pas)
        {
            string msg_to_send;
            if (type < 9)
            {
                msg_to_send = "1";
            }
            else
            {
                msg_to_send = "0";
                type = type - 10;
            }
            msg_to_send += Find_adress(nom);

            
            msg_to_send += Conv_1023_vers_10bits(value, type, pas);
            if (msg_to_send.Length == 18)
            {
                return AsynchronousClient.Send_data(msg_to_send);
            }
            
            /*
            if (msg_to_send.Substring(0, 1) == "1")
            {
                msg_to_send += Conv_1023_vers_10bits(value, type, pas);
                if (msg_to_send.Length == 18)
                {
                    return AsynchronousClient.Send_data(msg_to_send);
                }
            }
            else if (msg_to_send.Substring(0, 1) == "0")
            {
                msg_to_send += "0000000000";
                if (msg_to_send.Length == 18)
                {
                    return AsynchronousClient.Send_data(msg_to_send);
                }
            }*/
            return "-1";
        }



        //----------------------------------------------------------------------
        //Fonction pour gérer l'envoie des info utile pour les différents modes
        //
        //serveur = contient le socket
        //parent = la feuille qui a appeler
        //type = le type d'envoie DP = double pulse, BO = boucle ouverte
        //----------------------------------------------------------------------
        public static string Send_info(Control parent, string type)
        {
            List<Control> control_T = new List<Control>();
            control_T = GValeurForm.Crealist(parent);
            string msg_to_send;


            for (int i = 0; i < listadresse.Count; i++) // fait le tour de toute les adresses
            {
                if (listadresse[i].Use == type) // si l'adresse est utiles pour le mode type alors
                {
                    foreach (Control ct in control_T) // on recherche l'objet correspondant
                    {
                        if (ct.Name == listadresse[i].Nom) // des qu'on le trouve
                        {
                            msg_to_send = "1" + listadresse[i].Adresse; // on créer la valeur a envoyer avec un 1 + l'adresse
                            if (listadresse[i].Bin)//si la valeur est déjà en binaire
                            {
                                msg_to_send += ct.Text;//on l'envoie tel quelle
                            }
                            else //sinon on la convertie
                            {
                                msg_to_send += Conv_1023_vers_10bits(ct.Text, 1, 1); // + le message transformé en 10 bits en sachant que le mot et déjà en forme on a pas besoin de pas           
                            }
                            string send_ok = AsynchronousClient.Send_data(msg_to_send); // On l'envoie
                            if (send_ok == "-1")
                            {//si une erreur durant le transfert
                                // RENVOYER L EXPLICATION DE L ERREUR
                                return listadresse[i].Exp;
                                //return "-1";
                            }
                            
                        }
                    }
                }
            }
            return "";
        }

        #region recherchevaleur

        //----------------------------------------------------------------------
        //Fonction qui donne l'adresse suivant le nom
        //
        //nom => nom du controls dont on veux l'adresse FPGA
        //----------------------------------------------------------------------
        private static string Find_adress(string nom)
        {
            for (int i = 0; i < listadresse.Count; i++)
            {
                if (listadresse[i].Nom == nom)
                    return listadresse[i].Adresse;

            }
            return "00000"; // si rien trouvé   
        }

        //----------------------------------------------------------------------
        //Fonction qui donne le nom correspondant à une adresse
        //
        //adresse => adresse FPGA dont on veux le nom du control
        //----------------------------------------------------------------------
        private static string Find_nom(string adresse)
        {
            for (int i = 0; i < listadresse.Count; i++)
            {
                if (listadresse[i].Adresse == adresse)
                    return listadresse[i].Nom;

            }
            return "Rien"; // si rien trouvé   
        }

        //----------------------------------------------------------------------
        //Fonction qui donne le pas correspondant à une adresse
        //
        //adresse => adresse FPGA dont on veux le nom du control
        //----------------------------------------------------------------------
        private static double Find_pas(string adresse)
        {
            for (int i = 0; i < listadresse.Count; i++)
            {
                if (listadresse[i].Adresse == adresse)
                    return listadresse[i].Pas;

            }
            return -1; // si rien trouvé   
        }

        //----------------------------------------------------------------------
        //Fonction qui donne le pas correspondant à une adresse
        //
        //adresse => adresse FPGA dont on veux le nom du control
        //----------------------------------------------------------------------
        private static bool Find_bin(string adresse)
        {
            for (int i = 0; i < listadresse.Count; i++)
            {
                if (listadresse[i].Adresse == adresse)
                    return listadresse[i].Bin;

            }
            return false; // si rien trouvé   
        }

        //----------------------------------------------------------------------
        //Fonction pour trouver le bon control
        //
        //ct le control
        //nom le discréminent
        //----------------------------------------------------------------------
        private bool Predicat_ct_nom(Control ct)
        {
            if (ct.Name == "nom")
                return true;
            return false;
        }

        #endregion

        //Finitialise toute les valeurs d'adresse , les noms correspondants et leurs utilités pour les différents modes
        //----------------------------------------------------------------------
        public static void Init_adress_nom()
        {
            listadresse = new List<AdresseFPGA>()
            {
                new AdresseFPGA() {Nom = "tb_val_0", Adresse ="0000000", Exp = "B Nom de la carte"}, //nom de la carte
                
                


                //---------------------
                //Mode boucle
                //---------------------
                
                new AdresseFPGA() {Nom = "tb_val_1", Adresse ="0000001" ,Use = "BO", Exp = "D valeur initiale du triangle pour le bras A"},//D valeure initial du triangleA
                new AdresseFPGA() {Nom = "tb_val_2", Adresse ="0000010" ,Use = "BO", Exp = "D valeur initiale du triangle pour le bras B"},//D valeure initial du triangleB
                new AdresseFPGA() {Nom = "tb_val_3", Adresse ="0000011" ,Use = "BO", Exp = "D valeur initiale du triangle pour le bras C"},//D valeure initial du triangleC
                new AdresseFPGA() {Nom = "tb_val_4", Adresse ="0000100" ,Use = "BO", Exp = "D valeur initiale du triangle pour le bras D"},//D valeure initial du triangleD
                new AdresseFPGA() {Nom = "tb_val_5", Adresse ="0000101" ,Use = "ALL",Bin = true, Exp = "B { SensA;SensB;SensC;SensD;ActifA;ActifB;ActifC;ActifD;NivActif;Rouecodeuse_tps_mort}"},//B { SensA;SensB;SensC;SensD;ActifA;ActifB;ActifC;ActifD;NivActif;Rouecodeuse_tps_mort}
                new AdresseFPGA() {Nom = "tb_val_6", Adresse ="0000110" ,Use = "BO", Exp = "D consigne max"},//D consigne max
                new AdresseFPGA() {Nom = "tb_val_7", Adresse ="0000111" ,Use = "BO", Exp = "D consigne min"},//D consigne min
                new AdresseFPGA() {Nom = "tb_val_8", Adresse ="0001000" ,Use = "ALL", Pas = 5, Exp = "D temps mort * 5 ms"},//D temps mort * 5ms
                
                //-----------
                //double pulse
                //-----------
                new AdresseFPGA() {Nom = "tb_val_9", Adresse ="0001001" ,Use = "DP", Pas = 0.1, Exp = "D durée du pulse 1 * 100 ms"},//D Durée du pulse 1 * 100ms
                new AdresseFPGA() {Nom = "tb_val_10", Adresse ="0001010" ,Use = "DP", Pas = 0.1, Exp = "D durée du pulse 2 * 100 ms"},//D Durée du pulse 2 * 100ms
                new AdresseFPGA() {Nom = "tb_val_11", Adresse ="0001011" ,Use = "DP", Pas = 0.1, Exp = "D durée du pulse 3 * 100 ms"},//D Durée du pulse 3 * 100ms
                new AdresseFPGA() {Nom = "tb_val_12", Adresse ="0001100" ,Use = "ALL",Bin = true, Exp = "B {DPA;DPB;DPC;DPD;TransistorSelect;00000}"},//B {DPA;DPB;DPC;DPD;TransistorSelect;00000}

                //-----------
                //choix freq
                //-----------
                new AdresseFPGA() {Nom = "tb_val_13", Adresse ="0001101" ,Use = "BO", Exp = "D Division de la fréquence"},

                //--------------
                //consigne & can
                //--------------
                new AdresseFPGA() {Nom = "tb_val_14", Adresse ="0001110" ,Use = "BO", Exp = "D Consigne1"},//D Consigne1
                new AdresseFPGA() {Nom = "tb_val_15", Adresse ="0001111" ,Use = "OP", Exp = "D Consigne2"},//D Consigne2
                new AdresseFPGA() {Nom = "tb_val_16", Adresse ="0010000" ,Use = "BO", Exp = "D Min CAN avant erreur"},//D Min CAN avant erreur
                new AdresseFPGA() {Nom = "tb_val_17", Adresse ="0010001" ,Use = "BO", Exp = "D Max CAN avant erreur"},//D Max CAN avant erreur
                new AdresseFPGA() {Nom = "tb_val_18", Adresse ="0010010" ,Use = "BO",Bin = true, Exp = "B {Chxcons1(3b); Chxcons2(3bit);Chxcons3(3b)}"},
                                //

                new AdresseFPGA() {Nom = "tb_val_19", Adresse ="0010011" ,Use = "BO",Bin = true, Exp = "B MemError sur le FPGA {CAN4;CAN3;CAN2;CAN1;DétecterrorCAN1;CAN2;CAN3;CAN4}"},
                                //

                new AdresseFPGA() {Nom = "tb_val_20", Adresse ="0010100", Exp = "D Mesure CAN1"},//D Mesure CAN1
                new AdresseFPGA() {Nom = "tb_val_21", Adresse ="0010101", Exp = "D Mesure CAN2"},//D Mesure CAN2
                new AdresseFPGA() {Nom = "tb_val_22", Adresse ="0010110", Exp = "D Mesure CAN3"},//D Mesure CAN3
                new AdresseFPGA() {Nom = "tb_val_23", Adresse ="0010111", Exp = "D Mesure CAN4"},//D Mesure CAN4
                new AdresseFPGA() {Nom = "tb_val_24", Adresse ="0011000", Use ="BF", Exp = "D r0PID1 venant de kp"},//D r0PID1 venant de kp
                new AdresseFPGA() {Nom = "tb_val_25", Adresse ="0011001", Use ="BF", Exp = "D r1PID1 venant de ki"},//D r1PID1 venant de ki
                new AdresseFPGA() {Nom = "tb_val_26", Adresse ="0011010", Use ="BF", Exp = "D r2PID1 venant de kd"},//D r2PID1 venant de kd
                new AdresseFPGA() {Nom = "tb_val_27", Adresse ="0011011", Use ="BF", Exp = "D r0PID2 venant de kp"},//D r0PID2 venant de kp
                new AdresseFPGA() {Nom = "tb_val_28", Adresse ="0011100", Use ="BF", Exp = "D r1PID2 venant de ki"},//D r1PID2 venant de ki
                new AdresseFPGA() {Nom = "tb_val_29", Adresse ="0011101", Use ="BF", Exp = "D r2PID2 venant de kd"},//D r2PID2 venant de kd
                new AdresseFPGA() {Nom = "tb_val_30", Adresse ="0011110", Use ="BF", Exp = "D Valeur cible PID1"},//D Valeur cible PID1
                new AdresseFPGA() {Nom = "tb_val_31", Adresse ="0011111", Use ="BF", Exp = "D Valeur cible PID2"},//D Valeur cible PID2
                new AdresseFPGA() {Nom = "tb_val_32", Adresse ="0100000", Use ="BO", Exp = "D Valeur max dans les triangles (tout est sera mis au bon ratio)"},//D Valeur max dans les triangles (tout est sera mis au bon ratio)
                new AdresseFPGA() {Nom = "tb_val_33", Adresse ="0100001", Use ="BO", Bin = true, Exp = "B ou va la consigne{brasA;brasB;brasC;brasD;000000}"},//B ou va la consigne{brasA;brasB;brasC;brasD;000000}
                                                                                      //-- "WXYZ" W = bras4 ; x bras3; y bras2; z bras1
                                                                                      //--'1' = cons_A et 0 = cons_B

                new AdresseFPGA() {Nom = "tb_val_34", Adresse ="0100010", Use ="SF1", Exp = "D Diviseur de fréquence Consigne sinus FPGA 1"},//SF sinus FPGA
                new AdresseFPGA() {Nom = "tb_val_35", Adresse ="0100011", Use ="SF2", Exp = "D Diviseur de fréquence Consigne sinus FPGA 2"},

                //--------------
                //mode superposition
                //--------------

                new AdresseFPGA() {Nom = "tb_val_36", Adresse ="0100100", Use ="BO", Exp = "D Valeur max du triangle pour Bras 1"},//
                new AdresseFPGA() {Nom = "tb_val_37", Adresse ="0100101", Use ="BO", Exp = "D Valeur min du triangle pour Bras 1"},
                new AdresseFPGA() {Nom = "tb_val_38", Adresse ="0100110", Use ="BO", Exp = "D Valeur max du triangle pour Bras 2"},
                new AdresseFPGA() {Nom = "tb_val_39", Adresse ="0100111", Use ="BO", Exp = "D Valeur min du triangle pour Bras 2"},
                new AdresseFPGA() {Nom = "tb_val_40", Adresse ="0101000", Use ="BO", Exp = "D Valeur max du triangle pour Bras 3"},
                new AdresseFPGA() {Nom = "tb_val_41", Adresse ="0101001", Use ="BO", Exp = "D Valeur min du triangle pour Bras 3"},
                new AdresseFPGA() {Nom = "tb_val_42", Adresse ="0101010", Use ="BO", Exp = "D Valeur max du triangle pour Bras 4"},
                new AdresseFPGA() {Nom = "tb_val_43", Adresse ="0101011", Use ="BO", Exp = "D Valeur min du triangle pour Bras 4"},


                //--------------
                //mode multiconsigne
                //--------------
                
                new AdresseFPGA() {Nom = "tb_val_44", Adresse ="0101100", Use ="BF", Exp = "D Consigne max2"},
                new AdresseFPGA() {Nom = "tb_val_45", Adresse ="0101101", Use ="BF", Exp = "D Consigne min2"},
                new AdresseFPGA() {Nom = "tb_val_46", Adresse ="0101110", Use ="TRI", Exp = "D Consigne max3"},
                new AdresseFPGA() {Nom = "tb_val_47", Adresse ="0101111", Use ="TRI", Exp = "D Consigne min3"},
                new AdresseFPGA() {Nom = "tb_val_48", Adresse ="0110000", Use ="TRI", Exp = "D consigne3"},
                new AdresseFPGA() {Nom = "tb_val_49", Adresse ="0110001", Use ="SF3", Exp = "D Diviseur de fréquence Consigne sinus FPGA 3"},
                new AdresseFPGA() {Nom = "tb_val_50", Adresse ="0110010", Use ="", Exp = "D r0PID3 venant de kp"},
                new AdresseFPGA() {Nom = "tb_val_51", Adresse ="0110011", Use ="", Exp = "D r1PID3 venant de ki"},
                new AdresseFPGA() {Nom = "tb_val_52", Adresse ="0110100", Use ="", Exp = "D r2PID3 venant de kd"},
                new AdresseFPGA() {Nom = "tb_val_53", Adresse ="0110101", Use ="", Exp = "D Valeur cible PID3"},
                new AdresseFPGA() {Nom = "tb_val_54", Adresse ="0110110", Use ="BO", Exp = "D Amplitude1"},
                new AdresseFPGA() {Nom = "tb_val_55", Adresse ="0110111", Use ="BF", Exp = "D Amplitude2"},
                new AdresseFPGA() {Nom = "tb_val_56", Adresse ="0111000", Use ="TRI", Exp = "D Amplitude3"},
                new AdresseFPGA() {Nom = "tb_val_57", Adresse ="0111001", Use ="BO", Exp = "D OffSet1"},
                new AdresseFPGA() {Nom = "tb_val_58", Adresse ="0111010", Use ="BF", Exp = "D OffSet2"},
                new AdresseFPGA() {Nom = "tb_val_59", Adresse ="0111011", Use ="TRI", Exp = "D OffSet3"},
                new AdresseFPGA() {Nom = "tb_val_60", Adresse ="0111100", Use ="BO", Exp = "D Position de départ Consigne sinus 1"},
                new AdresseFPGA() {Nom = "tb_val_61", Adresse ="0111101", Use ="BF", Exp = "D Position de départ Consigne sinus 2"},
                new AdresseFPGA() {Nom = "tb_val_62", Adresse ="0111110", Use ="TRI", Exp = "D Position de départ Consigne sinus 3"},
                new AdresseFPGA() {Nom = "tb_val_63", Adresse ="0111111", Use ="MESURE_TAB_FPGA", Exp = "D Position de départ Consigne sinus 3"},
                new AdresseFPGA() {Nom = "tb_val_64", Adresse ="1000000", Use ="MESURE_TAB_FPGA", Exp = "D Position de départ Consigne sinus 3"},
            };
        }

        //----------------------------
        //demande toute les valeurs du FPGA
        //il faut faire x +1 requête car les retour on un tour de retard
        //
        //parent la feuille (donc this)
        //client le socket de connection
        //----------------------------
        public static string Reception(Control parent)
        {
            List<Control> control_T = control_T = GValeurForm.Crealist(parent); //créer une liste qui contient tout les Textbox, checkbox et combobox
            List<string> retour = new List<string>();
            for (int i = 0; i < listadresse.Count; i++)
            {
                retour.Add(AsynchronousClient.Send_data("0" + listadresse[i].Adresse + "0000000000")); //envoie une requête d'information de toute les adresse une par une
                if (retour[i] == "-1")//si erreur dans le transfert
                {
                    return "-1";//l'envoi plus haut
                }
                if (i > 0) //des la seconde valeur, car la première valeur dépendra du dernier envoie avant cette fonction
                {
                    Recuperation(control_T, retour[i]);//lance la récupération
                }
            }
            retour.Add(AsynchronousClient.Send_data("000000000000000000"));//envoie une valeur de plus pour récupérer la dernière valeur.
            Recuperation(control_T, retour[retour.Count - 1]);//lance la récupération
            if (retour[retour.Count-1] == "-1")//si erreur dans le transfert
            {
                return "-1";//l'envoi plus haut
            }
            return "ok";
        }

        //----------------------------
        //Fonction qui attribut la valeur reçu au checkbox textbox et index correspondant
        //
        //control_T list de tout les controls
        //mot  valeur reçu sous format Adresse 5 bits et valeur 10 bits
        //----------------------------
        private static void Recuperation(List<Control> control_T, string mot)
        {
            string adresse = mot.Substring(0, 7);//récupére l'adresse
            string valeur = mot.Substring(7, 10);//récupére la valeur
            string nom = Find_nom(adresse);//récupére le nom
            double pas = Find_pas(adresse);//récupère le pas
            //cas général pour toute les textbox
            if (Find_bin(adresse)) // si l'adresse en question demande un bin
            {
                control_T.Find(delegate (Control a) { return a.Name == nom; }).Text = valeur;//donne la valeur tel quel
            }
            else//sinon
            {
                control_T.Find(delegate (Control a) { return a.Name == nom; }).Text = Conv_1023_vers_10bits(valeur, 0, 1); //met la valeur mise en forme dans la textbox
            }
            control_T.Find(delegate (Control a) { return a.Name == nom; }).Select(); //selectionne la case pour le cas ou en validation quelque chose se produit (calcul etc)
        }

        //----------------------------
        //Fonction pour envoyer automatiquement les nouvelles valeurs
        //
        //sender = la textbox qui envoi une valeur
        //client = le socket qui sert à communiquer
        //----------------------------
        public static string Send_auto(object sender)
        {
            if (((TextBox)sender).Text != "")//si la text box n'est pas vide
            {
                string msg_to_send = "1";//choisir le mode écriture
                string adress = Find_adress(((TextBox)sender).Name);
                msg_to_send += adress;//stocker l'adresse du sender
                if (Find_bin(adress))//voir si le text est déjà binaire
                {
                    if (((TextBox)sender).Text.Length == 10)//si le text fait bien 10 caractère
                        msg_to_send += ((TextBox)sender).Text; //rajouter le text
                    else
                        return "-1";//sinon on ne fait rien
                }
                else//si non binaire
                {
                    try//try pour au cas ou l'utilisateur à fait n'importe quoi
                    {//vérifie si le text est >= 0 et inferrieur 1024
                        if (int.Parse(((TextBox)sender).Text) >= 0 && int.Parse(((TextBox)sender).Text) < 1024)
                            msg_to_send += Conv_1023_vers_10bits(((TextBox)sender).Text, 1, 1);
                        else
                            return "-1";//sinon ne fait rien
                    }
                    catch
                    {
                        return "-1";//si une erreur ne fait rien
                    }
                }
                msg_to_send += Find_adress(((TextBox)sender).Name);
                string send_ok = AsynchronousClient.Send_data(msg_to_send);
                if (send_ok == "-1")//si erreur dans le transfert
                {
                    return "-1"; // envoi plus haut
                }
                return "";
            }
            return "-1";
            
        }

        //----------------------------
        //Fonction pour récupérer la valeur des cans
        //
        //tb = la textbox qui va recevoir la valeur
        //client = le socket qui sert à communiquer
        //----------------------------
        public static string Mesure_CAN(Control tb)
        {
            //envoie la requête de mesure pour le composant TB
            string msg_to_send = "0" + listadresse.Find(delegate (AdresseFPGA a) { return a.Nom == tb.Name; }).Adresse + "0000000000";

            //return les 10 dernier bits retourner par send et les convertie en décimal
            string msg_recu = AsynchronousClient.Send_data(msg_to_send);
            if (msg_recu == "-1")//si erreur dans le transfert
            {
                return "-1";//l'envoi plus haut
            }
            return Conv_1023_vers_10bits(msg_recu.Substring(msg_recu.Length - 13, 10), 0, 1); //ne pas oublié les caratère retour chariot
        }

    }


}
