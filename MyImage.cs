using System;
using System.Collections.Generic;
using System.IO;

//projet HU Stella et HÖHENBERGER Enzo
namespace Probleme_Scientifique
{
    public class MyImage
    {
        #region Instances et Constructeurs

        //attributs
        public RGB[,] matriceRGB;
        string typeImage;
        int largeur;
        int hauteur;
        int tailleFichier;
        int tailleOffset;
        int nbBits;
        byte[] tabTaille = new byte[4];
        byte[] tabTailleOffset = new byte[4];
        byte[] tabLargeur = new byte[4];
        byte[] tabHauteur = new byte[4];
        byte[] tabNbBits = new byte[2];
        byte[] Header = new byte[54];


        public string TypeImage { get { return typeImage; } }
        public int Hauteur
        {
            get { return hauteur; }
            set { this.hauteur = value; }
        }
        public int Largeur
        {
            get { return largeur; }
            set { this.largeur = value; }
        }
        public RGB[,] MatriceRGB
        {
            get
            {
                return matriceRGB;
            }
            set
            {
                this.matriceRGB = value;
            }
        }

        //Constructeur MyImage

        /// <summary>
        /// Cette fonction nous permet de prendre les informations a propos du fichier en parametre qui sont mis dans un tableau.
        /// Puis chaque information que l'on aura besoin prennent leurs valeur en byte approprié a ce qu'ils decrivent: 
        ///     ex: les bytes pour la hauteur de l'image sont recupere dans un tableau pour la hauteur 
        ///         (que l'on converti en Int pour les modification avec les fonctions pour le traitement d'image)
        /// </summary>
        /// <param name="MonFichier"></param>
        public MyImage(string MonFichier)
        {
            byte[] Myfile = File.ReadAllBytes(MonFichier);

            typeImage = "";
            for (int i = 0; i < 2; i++)
            {
                char a = (char)Myfile[i];
                typeImage += a;
            }

            //récupération de la taille du fichier
            for (int i = 2; i < 6; i++)
            {
                tabTaille[i - 2] = Myfile[i];
            }
            tailleFichier = Convertir_Endian_To_Int(tabTaille);

            //récupération de la taille de l'offset 
            for (int i = 10; i < 14; i++)
            {
                tabTailleOffset[i - 10] = Myfile[i];
            }
            tailleOffset = Convertir_Endian_To_Int(tabTailleOffset);

            //récupération de la largeur
            for (int i = 18; i < 22; i++)
            {
                tabLargeur[i - 18] = Myfile[i];
            }
            largeur = Convertir_Endian_To_Int(tabLargeur);

            //récupération de la hauteur           
            for (int i = 22; i < 26; i++)
            {
                tabHauteur[i - 22] = Myfile[i];
            }
            hauteur = Convertir_Endian_To_Int(tabHauteur);

            //récupération du nombre de bits de couleur           
            for (int i = 28; i < 30; i++)
            {
                tabNbBits[i - 28] = Myfile[i];
            }
            nbBits = Convertir_Endian_To_Int(tabNbBits);

            for (int i = 29; i < 54; i++)
            {
                Header[i - 29] = Myfile[i];
            }

            //récupération de l'image
            int index = 54;
            matriceRGB = new RGB[hauteur, largeur];
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    matriceRGB[i, j] = new RGB(Convert.ToInt32(Myfile[index]), Convert.ToInt32(Myfile[index + 1]), Convert.ToInt32(Myfile[index + 2]));
                    index += 3;
                }
                while ((index - 54) % 4 != 0)
                {
                    index++;
                }
            }
        }

        /// <summary>
        /// Constructeur Vide pour pouvoir créer de toute pièce notre Fractale.
        /// </summary>
        public MyImage()
        {

        }
        #endregion

        #region Methodes
        //méthodes

        /// <summary>
        /// Cette fonction permet de modifier un fichier/ les attributs donne pour l'image.
        /// En changeant les attribut dans les differentes fonctions, nous pouvons creer une image avec ces  memes attributs modifies.
        /// Elle va donc creer un fichier image qui va pouvoir etre lisible par l'ordinateur.
        /// </summary>
        /// <param name="file">Le string file est le nom que portera le nouveau fichier/image apres modification des attributs</param>
        public void From_Image_To_File(string file)
        {
            byte[] result = new byte[tailleOffset + 3 * matriceRGB.Length];

            List<byte> File1 = new List<byte>();

            File1.Add(Convert.ToByte("66"));
            File1.Add(Convert.ToByte("77"));

            for (int i = 2; i < 6; i++)
            {
                File1.Add(Convert.ToByte(tabTaille[i - 2]));
            }
            for (int i = 6; i < 10; i++)
            {
                File1.Add(Convert.ToByte("0"));
            }
            File1.Add(Convert.ToByte("54"));
            for (int i = 11; i < 14; i++)
            {
                File1.Add(Convert.ToByte("0"));
            }
            File1.Add(Convert.ToByte("40"));
            for (int i = 15; i < 18; i++)
            {
                File1.Add(Convert.ToByte("0"));
            }
            tabLargeur = Convertir_Int_To_Endian(largeur);
            for (int i = 18; i < 22; i++)
            {
                File1.Add(Convert.ToByte(tabLargeur[i - 18]));
            }
            tabHauteur = Convertir_Int_To_Endian(hauteur);
            for (int i = 22; i < 26; i++)
            {
                File1.Add(Convert.ToByte(tabHauteur[i - 22]));
            }
            File1.Add(Convert.ToByte("1"));
            File1.Add(Convert.ToByte("0"));
            File1.Add(Convert.ToByte("24"));
            for (int i = 29; i < 54; i++)
            {
                File1.Add(Convert.ToByte(Header[i - 29]));
            }
            for (int i = 0; i < matriceRGB.GetLength(0); i++)
            {
                for (int j = 0; j < matriceRGB.GetLength(1); j++)
                {
                    File1.Add(Convert.ToByte(matriceRGB[i, j].Bleu));
                    File1.Add(Convert.ToByte(matriceRGB[i, j].Vert));
                    File1.Add(Convert.ToByte(matriceRGB[i, j].Rouge));
                }
                for (int a = 0; a < matriceRGB.GetLength(1) % 4; a++)
                {
                    File1.Add(Convert.ToByte(byte.MinValue));
                }
            }
            File.WriteAllBytes(file, File1.ToArray());
        }

        /// <summary>
        /// Convertisseur qui transforme les informations sur les differents attributs d'une image en byte, en int.
        ///     ex: On lite un attribut "156,0,0,0" et le convertisseur nous donne la valeur int de cet endian, sois "156".
        /// </summary>
        /// <param name="tableau">tableau donne en prenant les informations du header</param>
        /// <returns>La valeur (int) correspondant a la suite en Edian donne en parametre</returns>
        public int Convertir_Endian_To_Int(byte[] tableau) //méthode de conversion endian en entier
        {
            int NombreInt = 0;
            for (int i = 0; i < tableau.Length; i++)
            {
                NombreInt = NombreInt + (tableau[i] * (int)Math.Pow(256, i));
            }
            return NombreInt;
        }

        /// <summary>
        /// Cette fonction va nous permettre de passer in entier en int en Endian.
        /// </summary>
        /// <param name="val">Valeur entiere que l'on veut transformer en endian</param>
        /// <returns>Une suite de nombre (d'octet) en little endian qui correspond a la val</returns>
        public byte[] Convertir_Int_To_Endian(int val) //méthode de conversion entier en endian
        {
            byte[] Valeur = new byte[4];
            Valeur[0] = (byte)val;
            Valeur[1] = (byte)(((uint)val >> 8) & 0xFF);
            Valeur[2] = (byte)(((uint)val >> 16) & 0xFF);
            Valeur[3] = (byte)(((uint)val >> 24) & 0xFF);
            return Valeur;
        }
        #endregion

        #region Traitements

        /// <summary>
        /// Cette fonction nous permet de passer une image de couleurs en image ayant des nuances de gris.
        /// Cela est permis en variant l'intensite/l'éclaircissecement de la valeur de gris.
        /// Tout d'abord, nous trovons du gris en ayant la meme valeur (RGB) de Rouge, de Vert et de Bleu.
        /// On en fait donc la moyenne (de rouge, vert et bleu) pour trouver la valeur de gris la plus correct pour chaque pixel. (Moyenne faite dans la classe RGB)
        /// Enfin, nous allon traverser la matrice et la valeur de Rouge, Vert et Bleu sera change en fonction de la moyenne des 3 valeurs pour donne une valeur de gris.
        /// </summary>
        public void Monochrome_Gris() //méthode pour rendre l'image en nuances de gris
        {
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    matriceRGB[i, j].Gris();
                }
            }
        }

        /// <summary>
        /// Cette fonction va nous permettre de retourner une image telle que si on la voyait dans un miroir.
        /// Il faut donc par rapport a un axe de milieux inverser successivement les pixels de la partie gauche a droite.
        /// </summary>
        public void Miroir() //méthode pour ajouter un effet mirroir à l'image
        {
            RGB[,] MatriceTemp = matriceRGB;

            int milieu;
            if (this.largeur % 2 == 0)  //recuperation du milieu
            {
                milieu = this.largeur / 2;
            }
            else
            {
                milieu = (this.largeur - 1) / 2;
            }

            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < milieu; j++)        //Miroir fait, il faut faire attention aux couleurs -> Dit RVB mais etudie BVR
                {
                    RGB DeuxiemeMatrice = new RGB(this.matriceRGB[i, j].Bleu, this.matriceRGB[i, j].Vert, this.matriceRGB[i, j].Rouge);
                    this.matriceRGB[i, j] = MatriceTemp[i, this.largeur - j - 1];
                    this.matriceRGB[i, this.largeur - j - 1] = DeuxiemeMatrice;
                }
            }
            //pas besoin de faire des retour de valeurs: L'image nest pas changee
        }

        /// <summary>
        /// Cette fonction nous permet de passer une image en ensemple de pixels de noirs ou de blanc:
        ///     En prenant la valeur moyenne (valeur RGB) milieux entre le blanc et le noir, nous avons une une valeur limite.
        ///     De la, si la valeur moyenne de rouge, de vert et de bleu est plus forte ou plus faible que la valeur limite on change le pixel en blanc ou en noir.
        ///     On rappel que la couleur du blanc est (255, 255 ,255) et que celle du noir est (0, 0, 0).
        /// </summary>.
        public void Noir_Et_Blanc()
        {
            int Separation_Couleurs = (255 * 3) / 2;    //limite de separation entre les deux couleurs

            for (int i = 0; i < matriceRGB.GetLength(0); i++)
            {
                for (int j = 0; j < matriceRGB.GetLength(1); j++)
                {
                    int SommeDesCouleurs = matriceRGB[i, j].Rouge + matriceRGB[i, j].Vert + matriceRGB[i, j].Bleu; //Valeurs moyenne des couleurs de chaque pixel pour la comparer a la limite

                    if (SommeDesCouleurs < Separation_Couleurs) //decision du changement de couleur en fonction de la moyenne avant
                    {
                        matriceRGB[i, j].Rouge = 0;
                        matriceRGB[i, j].Vert = 0;      //noir
                        matriceRGB[i, j].Bleu = 0;
                    }
                    else
                    {
                        matriceRGB[i, j].Rouge = 255;
                        matriceRGB[i, j].Vert = 255;    //blanc
                        matriceRGB[i, j].Bleu = 255;
                    }
                }
            }
        }

        /// <summary>
        /// L'innovation a pour but d'etudier l'image.(D'un point de vue photographique mais aussi artistique)
        ///     Dans beaucoup de cas, il est interessant d'avoir quelles couleurs sont le plus presentes:
        ///         - En photographie, cela nous permet de mieux connaitres les couleurs qui composent un endroit, et d'appliquer certains filtres si necessaires
        ///             (ex: si une photo ressort trop jaune, il est convenable d'appliquer un filtre bleu pour avoir une image plus agreable/coherante avec ce que l'on voit)
        ///         - En art, c'est utile pour connaitre la palette de couleurs la plus utile/utilisee dans l'image pour la recreer ou l'etudier
        ///             (ex: Pour voir la composition des formes au sein de l'image, des lignes qui ressortent de l'image,...)
        /// 
        /// Ce filre est fait en mettant des "bornes" pour les couleurs et verifiant certaines conditions supplementaires pour les couleurs comme le Cyan, le Jaune et le Magenta.
        /// </summary>
        public void Innovation() //methode pour passer limage en noir et blanc en fonction de la moyenne de la valeur de couleur de chaque pixel 
        {
            for (int i = 0; i < matriceRGB.GetLength(0); i++)
            {
                for (int j = 0; j < matriceRGB.GetLength(1); j++)
                {
                    if (matriceRGB[i, j].Vert > 230 && matriceRGB[i, j].Rouge > 230 && matriceRGB[i, j].Rouge > 230) //limitation pour les pixel M blanc (M=majoritarement)
                    {
                        matriceRGB[i, j].Rouge = 255;
                        matriceRGB[i, j].Vert = 255;
                        matriceRGB[i, j].Bleu = 255;
                    }
                    if (matriceRGB[i, j].Vert < 25 && matriceRGB[i, j].Rouge < 25 && matriceRGB[i, j].Rouge < 25) //limitation pour les pixels M noir
                    {
                        matriceRGB[i, j].Rouge = 0;
                        matriceRGB[i, j].Vert = 0;
                        matriceRGB[i, j].Bleu = 0;
                    }
                    if (matriceRGB[i, j].Vert >= 0 && matriceRGB[i, j].Vert <= 255 && 0 <= matriceRGB[i, j].Rouge
                        && matriceRGB[i, j].Rouge <= 255 && 0 <= matriceRGB[i, j].Bleu && matriceRGB[i, j].Bleu <= 255
                        && matriceRGB[i, j].Rouge > matriceRGB[i, j].Vert && matriceRGB[i, j].Rouge <= matriceRGB[i, j].Bleu) //limitation pour les pixels M Magenta
                    {
                        matriceRGB[i, j].Rouge = 255;
                        matriceRGB[i, j].Vert = 0;
                        matriceRGB[i, j].Bleu = 255;
                    }
                    if (matriceRGB[i, j].Vert >= 0 && matriceRGB[i, j].Vert <= 255 && 0 <= matriceRGB[i, j].Rouge
                        && matriceRGB[i, j].Rouge <= 255 && 0 <= matriceRGB[i, j].Bleu && matriceRGB[i, j].Bleu <= 255
                        && matriceRGB[i, j].Rouge <= matriceRGB[i, j].Vert && matriceRGB[i, j].Rouge > matriceRGB[i, j].Bleu) //limitation pour les pixels M jaune
                    {
                        matriceRGB[i, j].Rouge = 255;
                        matriceRGB[i, j].Vert = 255;
                        matriceRGB[i, j].Bleu = 0;
                    }
                    if (matriceRGB[i, j].Vert >= 0 && matriceRGB[i, j].Vert <= 255 && 0 <= matriceRGB[i, j].Rouge
                        && matriceRGB[i, j].Rouge <= 255 && 0 <= matriceRGB[i, j].Bleu && matriceRGB[i, j].Bleu <= 255
                        && matriceRGB[i, j].Rouge > matriceRGB[i, j].Vert && matriceRGB[i, j].Rouge > matriceRGB[i, j].Bleu) //limitation pour les pixels M Rouge
                    {
                        matriceRGB[i, j].Rouge = 255;
                        matriceRGB[i, j].Vert = 0;
                        matriceRGB[i, j].Bleu = 0;
                    }
                    if (matriceRGB[i, j].Vert >= 0 && matriceRGB[i, j].Vert <= 255 && 0 <= matriceRGB[i, j].Rouge
                        && matriceRGB[i, j].Rouge <= 255 && 0 <= matriceRGB[i, j].Bleu && matriceRGB[i, j].Bleu <= 255
                        && matriceRGB[i, j].Vert > matriceRGB[i, j].Rouge && matriceRGB[i, j].Vert > matriceRGB[i, j].Bleu) //limitation pour les pixels M Vert
                    {
                        matriceRGB[i, j].Rouge = 0;
                        matriceRGB[i, j].Vert = 255;
                        matriceRGB[i, j].Bleu = 0;
                    }
                    if (matriceRGB[i, j].Vert >= 0 && matriceRGB[i, j].Vert <= 255 && 0 <= matriceRGB[i, j].Rouge
                        && matriceRGB[i, j].Rouge <= 200 && 0 <= matriceRGB[i, j].Bleu && matriceRGB[i, j].Bleu <= 255
                        && matriceRGB[i, j].Bleu > matriceRGB[i, j].Rouge && matriceRGB[i, j].Vert > matriceRGB[i, j].Rouge) //limitation pour les pixels M Bleu
                    {
                        matriceRGB[i, j].Rouge = 0;
                        matriceRGB[i, j].Vert = 0;
                        matriceRGB[i, j].Bleu = 255;
                    }
                    if (matriceRGB[i, j].Vert >= 0 && matriceRGB[i, j].Vert < 255 && 0 <= matriceRGB[i, j].Rouge
                        && matriceRGB[i, j].Rouge < 255 && 0 <= matriceRGB[i, j].Bleu && matriceRGB[i, j].Bleu < 255
                        && matriceRGB[i, j].Bleu > matriceRGB[i, j].Vert && matriceRGB[i, j].Bleu > matriceRGB[i, j].Rouge) //limitation pour les pixels M Cyan
                    {
                        matriceRGB[i, j].Rouge = 0;
                        matriceRGB[i, j].Vert = 255;
                        matriceRGB[i, j].Bleu = 255;
                    }
                }
            }
        }

        /// <summary>
        /// Pour agrandir une image, nous construisons une "nouvelle image" dont les dimensions vont etre multipliees par un coefficiant.
        ///     Une fois cela fait, nous allons transposer et copier chque pixel de la premiere image a l'image agrandie en verifiant que le pixel de couleur de la premiere image
        ///     sois copier au meme nombre de fois que le coefficiant dans la nouvelle matrice.
        ///     (ex: Soit la premiere ligne d'une image ayant deux pixels: [][] Dans la nouvelle matrice d'un agrandissment de deux, la premiere ligne sera telle : [][][][])
        ///   Enfin, nous retournons a la fin la nouvelle valeur de la taille de la matrice pour appliquer les modifications apportees. (ainsi que la matrice finale).
        /// </summary>
        /// <param name="Coefficient">coefficiant par lequel on va multiplier les bord de l'ancienne image pour creer la nouvelle image (ex: Si = 1, alors les bords ne changent pas)</param>
        public void Agrandir(int Coefficient)
        {
            RGB[,] ImageGrande = new RGB[hauteur * Coefficient, largeur * Coefficient];

            for (int ligne = 0; ligne < ImageGrande.GetLength(0); ligne++)
            {
                for (int colonne = 0; colonne < ImageGrande.GetLength(1); colonne++)
                {
                    ImageGrande[ligne, colonne] = matriceRGB[ligne / Coefficient, colonne / Coefficient];
                }
            }

            matriceRGB = ImageGrande;               //retour des nouveaux attributs de la nouvelle matrice
            hauteur = hauteur * Coefficient;
            largeur = largeur * Coefficient;
        }

        /// <summary>
        /// De la meme maniere que la matrice d'agrandissement, on va creer une nouvelle image et "copier" les differents pixels de couleurs mais dans le sens inverse.
        ///     (ex: Soit la premiere ligne d'une matrice: [][][][], un retrecissement de deux sera tel: [][])
        ///   Enfin, encore une fois, nous retournons les nouveaux attributs de l'image et la matrice finale.
        /// </summary>
        /// <param name="coefficient">coefficiant par lequel on va diviser les bord de l'ancienne image pour creer la nouvelle image</param>
        public void Retrecir(int coefficient)
        {
            RGB[,] ImagePetite = new RGB[hauteur / coefficient, largeur / coefficient];

            for (int ligne = 0; ligne < ImagePetite.GetLength(0); ligne++)
            {
                for (int colonne = 0; colonne < ImagePetite.GetLength(1); colonne++)
                {
                    ImagePetite[ligne, colonne] = matriceRGB[ligne * coefficient, colonne * coefficient];
                }
            }

            matriceRGB = ImagePetite;           //retour des nouveaux attributs de la nouvelle image
            hauteur = hauteur / coefficient;
            largeur = largeur / coefficient;
        }

        /// <summary>
        /// La fonction cacherImageDansAutre va permettre de prendre une image choisie, de modifier sa transparance et de la placer au sein d'une autre image
        /// et donc la cacher, car elle va se "fondre" dans cette derniere.
        /// Le but de la fonction est de prendre les bits forts de l'image supports et les bits faibles de l'image cachee et de les reunirs en sortie.
        /// On va pouvoir le faire, grace aux operateurs logique "Et" et "Ou" (tables de veritee)
        /// </summary>
        /// <param name="cachee">On prends en parametre l'image qui sera cachee dans l'image de support (Arriere-plan)</param>
        public void CacherImageDansAutre(MyImage cachee)
        {
            if (largeur >= cachee.largeur && hauteur >= cachee.hauteur)
            {
                int[] PrincipalRouge = new int[hauteur * largeur]; //Creation dun tableau pour tous les pixels de l'image Supprt
                int[] PrincipalVert = new int[hauteur * largeur];
                int[] PrincipalBleu = new int[hauteur * largeur];

                int[] CacheeRouge = new int[cachee.hauteur * cachee.largeur]; //Creation dun tableau pour tous les pixels de l'image cachee
                int[] CacheeVert = new int[cachee.hauteur * cachee.largeur];
                int[] CacheeBleu = new int[cachee.hauteur * cachee.largeur];

                int[] SortieRouge = new int[cachee.hauteur * cachee.largeur]; //Creation dun tableau pour tous les pixels de l'image de sortie
                int[] SortieVert = new int[cachee.hauteur * cachee.largeur];
                int[] SortieBleu = new int[cachee.hauteur * cachee.largeur];

                int position = 0;

                for (int i = 0; i < cachee.hauteur; i++)
                {
                    for (int j = 0; j < cachee.largeur; j++)
                    {
                        PrincipalRouge[position] = matriceRGB[i, j].Rouge & 240;  //Permet de garder les bits de poids forts de l'image support (a)
                        PrincipalVert[position] = matriceRGB[i, j].Vert & 240;
                        PrincipalBleu[position] = matriceRGB[i, j].Bleu & 240;

                        CacheeRouge[position] = cachee.matriceRGB[i, j].Rouge & 240;  //On recupere la valeurs de bits de poids forts de l'image cachee
                        CacheeRouge[position] = CacheeRouge[position] >> 4;  // Decaler a DROITE de 4 bits en tant que bits de poids faible (b)
                        CacheeVert[position] = cachee.matriceRGB[i, j].Vert & 240;
                        CacheeVert[position] = CacheeVert[position] >> 4;
                        CacheeBleu[position] = cachee.matriceRGB[i, j].Bleu & 240;
                        CacheeBleu[position] = CacheeBleu[position] >> 4;

                        SortieRouge[position] = PrincipalRouge[position] | CacheeRouge[position]; //on rassemble les bits pour avoir (a)(b)
                        SortieVert[position] = PrincipalVert[position] | CacheeVert[position];
                        SortieBleu[position] = PrincipalBleu[position] | CacheeBleu[position];

                        position++;
                    }
                }
                position = 0;
                for (int i = 0; i < cachee.hauteur; i++)
                {
                    for (int j = 0; j < cachee.largeur; j++)
                    {
                        matriceRGB[i, j].Rouge = SortieRouge[position]; //On modifie la matrice pour les nouvelles couleurs (ou se situe l'image cachee)
                        matriceRGB[i, j].Vert = SortieVert[position];
                        matriceRGB[i, j].Bleu = SortieBleu[position];

                        position++;
                    }
                }
            }
        }

        /// <summary>
        /// La fonction TrouverImageCachee va permettre de prendre une image cachee et de la refaire apparaitre pour retrouver ses couleurs originelles.
        /// Pour cela, nous allons faire le chemin inverse que celui dans la fonction de l'image cachee. A la place de cacher une image, nous allons faire en sorte
        /// de ne faire que paraitre l'image cachee.
        /// Pour cela, on prends les bits faibles de l'image cachee et nous les mettons en  bits forts avant de donner la nouvelle image.
        /// On va pouvoir le faire, grace aux operateurs logique "Et" et "Ou" (tables de veritee)
        /// </summary>
        /// <param name="cachee">On prends en parametre l'image est cachee dans l'image de support (Arriere-plan)</param>
        public void TrouverImageCachee(MyImage cachee)
        {
            int[] CacheeRouge = new int[hauteur * largeur];
            int[] CacheeVert = new int[hauteur * largeur];
            int[] CacheeBleu = new int[hauteur * largeur];

            int position = 0;
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {

                    CacheeRouge[position] = matriceRGB[i, j].Rouge & 15;  //permet de garder les bits de poids faible de l'image
                    CacheeRouge[position] = CacheeRouge[position] << 4;   //deplacer en tant que bits forts
                    CacheeVert[position] = matriceRGB[i, j].Vert & 15;
                    CacheeVert[position] = CacheeVert[position] << 4;
                    CacheeBleu[position] = matriceRGB[i, j].Bleu & 15;
                    CacheeBleu[position] = CacheeBleu[position] << 4;

                    position++;
                }
            }
            position = 0;
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    matriceRGB[i, j].Rouge = CacheeRouge[position];  //modification des nouvelles couleurs
                    matriceRGB[i, j].Vert = CacheeVert[position];
                    matriceRGB[i, j].Bleu = CacheeBleu[position];

                    position++;
                }
            }

            /*for (int i = 0; i < hauteur; i++)                       //Ajout de bord pour la "proprete de l'image
            {
                for (int j = cachee.largeur; j < largeur; j++)
                {
                    matriceRGB[i, j] = new RGB(0, 0, 0);
                }
            }
            for (int i = cachee.hauteur; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    matriceRGB[i, j] = new RGB(0, 0, 0);
                }
            }*/
        }

        /// <summary>
        /// La fonction rotation va permetre une rotation a un angle quelconque de l'image que l'on veux.
        /// Pour cela, nous determinons les nouvelles dimentions de l'image, et remplissons la nouvelle image de noir.
        /// Puis, nous deplacons chaque pixel sur une nouvelle image a une nouvelle position grace au systeme de coordonees polaires.
        /// enfin, nous rtetournons les nouvelles valeurs (qui ont etees modifiees) pour modifier la matrice de sortie.
        /// </summary>
        /// <param name="rad">Angle de rotation (en degres) dans le sens des aiguilles d'une montre</param>
        public void Rotation(double rad)
        {
            double angle = rad * Math.PI / 180;

            int NouvelleHauteur = (int)Math.Ceiling((Math.Abs(Math.Cos(angle)) * matriceRGB.GetLength(1) + Math.Abs(Math.Sin(angle)) * matriceRGB.GetLength(0)));
            int NouvelleLargeur = (int)Math.Ceiling((Math.Abs(Math.Sin(angle)) * matriceRGB.GetLength(0) + Math.Abs(Math.Cos(angle)) * matriceRGB.GetLength(1)));

            RGB[,] Rotation = new RGB[(int)NouvelleHauteur, (int)NouvelleLargeur];

            int ACentre_x = matriceRGB.GetLength(0) / 2;
            int ACentre_y = matriceRGB.GetLength(1) / 2;

            int NCentre_x = Rotation.GetLength(0) / 2;
            int NCentre_y = Rotation.GetLength(0) / 2;

            for (int i = 0; i < NouvelleHauteur; i++)
            {
                for (int j = 0; j < NouvelleLargeur; j++)
                {
                    Rotation[i, j] = new RGB(0, 0, 0);
                }
            }

            for (int x = 0; x < Rotation.GetLength(0); x++)
            {
                for (int y = 0; y < Rotation.GetLength(1); y++)
                {
                    double rayon = Math.Sqrt(Math.Pow(x - NCentre_x, 2) + Math.Pow(y - NCentre_y, 2));
                    double angleteta = Math.Atan2((y - NCentre_y), (x - NCentre_x)) - angle;

                    int a = (int)(ACentre_x + rayon * Math.Cos(angleteta));
                    int b = (int)(ACentre_y + rayon * Math.Sin(angleteta));

                    if (a < matriceRGB.GetLength(0) && b < matriceRGB.GetLength(1) && b >= 0 && a >= 0)
                    {
                        Rotation[x, y] = matriceRGB[a, b];
                    }
                }
            }
            largeur = NouvelleLargeur;
            hauteur = NouvelleHauteur;
            tailleFichier = 3 * NouvelleLargeur * NouvelleHauteur + tailleOffset;
            matriceRGB = Rotation;
        }
        public void Histogramme()
        {
            int[] tableauROUGE = new int[256];
            int[] tableauVERT = new int[256];
            int[] tableauBLEU = new int[256];

            for (int i = 0; i < matriceRGB.GetLength(0); i++) //comptage
            {
                for (int j = 0; j < matriceRGB.GetLength(1); j++)
                {
                    int bleu = matriceRGB[i, j].Bleu;
                    int vert = matriceRGB[i, j].Vert;
                    int rouge = matriceRGB[i, j].Rouge;

                    tableauBLEU[bleu]++;
                    tableauROUGE[rouge]++;
                    tableauVERT[vert]++;
                }
            }

            int max = 0;
            for (int i = 0; i < 256; i++)
            {
                if (tableauBLEU[i] > max)
                {
                    max = tableauBLEU[i];
                }
                if (tableauROUGE[i] > max)
                {
                    max = tableauROUGE[i];
                }
                if (tableauVERT[i] > max)
                {
                    max = tableauVERT[i];
                }
            }

            this.hauteur = 256;
            this.largeur = 256;
            this.tailleFichier = 54 + this.hauteur * this.largeur * 3;

            RGB[,] MatriceHistogramme = new RGB[this.hauteur, this.largeur];

            for (int i = 0; i < MatriceHistogramme.GetLength(0); i++) //matrice noir
            {
                for (int j = 0; j < MatriceHistogramme.GetLength(1); j++)
                {
                    MatriceHistogramme[i, j] = new RGB(0, 0, 0);
                }
            }

            for (int j = 0; j < 256; j++) //remplissage en fonction des valeurs obtenues
            {
                for (int i = 0; i < 256 + 1; i++)
                {
                    if (tableauBLEU[j] != 0 && tableauBLEU[j] * this.hauteur / max == i)
                    {
                        for (int a = 0; a < tableauBLEU[j] * this.hauteur / max; a++)
                        {
                            MatriceHistogramme[a, j].Bleu = 255;
                        }
                    }
                    if (tableauVERT[j] != 0 && tableauVERT[j] * this.hauteur / max == i)
                    {
                        for (int a = 0; a < tableauVERT[j] * this.hauteur / max; a++)
                        {
                            MatriceHistogramme[a, j].Vert = 255;
                        }
                    }
                    if (tableauROUGE[j] != 0 && tableauROUGE[j] * this.hauteur / max == i)
                    {
                        for (int a = 0; a < tableauROUGE[j] * this.hauteur / max; a++)
                        {
                            MatriceHistogramme[a, j].Rouge = 255;
                        }
                    }
                }
            }

            this.matriceRGB = MatriceHistogramme;
        }
        public void FractaleMandelbrot(double zoom, int couleur)
        {
            double BorneGaucheX = -2.1;
            double BorneDroiteX = 0.6;
            double BorneHauteY = -1.2;
            double BorneBasseY = 1.2;
            //double zoom = 1000; // pour une distance de 1 sur le plan, on a 100 pixel sur l'image
            int iteration_max = 50;

            int image_x = Convert.ToInt32((BorneDroiteX - BorneGaucheX) * zoom);
            int image_y = Convert.ToInt32((BorneBasseY - BorneHauteY) * zoom);

            RGB[,] Fractale = new RGB[image_x, image_y];

            for (int x = 0; x < image_x; x++)
            {
                for (int y = 0; y < image_y; y++)
                {

                    double NombreReel = x / zoom + BorneGaucheX;
                    double Nombrecomplexe = y / zoom + BorneHauteY;
                    double ComplexePartieR = 0;
                    double ComplexePartieI = 0;
                    int i = 0;

                    do
                    {
                        double tmp = ComplexePartieR;
                        ComplexePartieR = ComplexePartieR * ComplexePartieR - ComplexePartieI * ComplexePartieI + NombreReel;
                        ComplexePartieI = 2 * ComplexePartieI * tmp + Nombrecomplexe;
                        i++;

                    } while (ComplexePartieR * ComplexePartieR + ComplexePartieI * ComplexePartieI < 4 && i < iteration_max);


                    if (i == iteration_max)
                    {

                        Fractale[x, y] = new RGB(0, 0, 0);

                    }

                    else
                    {
                        if (couleur == 1)
                        {
                            Fractale[x, y] = new RGB((int)i * 255 / iteration_max, 0, 0);
                        }
                        if (couleur == 2)
                        {
                            Fractale[x, y] = new RGB(0, (int)i * 255 / iteration_max, 0);
                        }
                        if (couleur == 3)
                        {
                            Fractale[x, y] = new RGB(0, 0, (int)i * 255 / iteration_max);
                        }
                        if (couleur == 4)
                        {
                            Fractale[x, y] = new RGB((int)i * 255 / iteration_max, (int)i * 255 / iteration_max, 0);
                        }
                        if (couleur == 5)
                        {
                            Fractale[x, y] = new RGB((int)i * 255 / iteration_max, 0, (int)i * 255 / iteration_max);
                        }
                        if (couleur == 6)
                        {
                            Fractale[x, y] = new RGB(0, (int)i * 255 / iteration_max, (int)i * 255 / iteration_max);
                        }
                        if (couleur == 7)
                        {
                            Fractale[x, y] = new RGB((int)i * 255 / iteration_max, (int)i * 255 / iteration_max, (int)i * 255 / iteration_max);
                        }
                        if (couleur == 8)
                        {
                            Fractale[x, y] = new RGB((int)Math.Ceiling((Math.Abs(Math.Sin(i) * 25500) * i) % iteration_max * 2), (int)Math.Ceiling((Math.Abs(Math.Sin(i) * 25500) * i) % iteration_max * 2), (int)Math.Ceiling((Math.Abs(Math.Sin(i) * 25500) * i) % iteration_max * 2));
                        }
                    }
                }
            }
            hauteur = image_x;
            largeur = image_y;
            tailleFichier = 3 * image_y * image_x + tailleOffset;
            matriceRGB = Fractale;
        }
        public void FractaledeJulia(double zoom, int couleur)
        {
            double BorneGaucheX = -1;
            double BorneDroiteX = 1;
            double BorneHauteY = -1.2;
            double BorneBasseY = 1.2;
            //double zoom = 1000; // pour une distance de 1 sur le plan, on a 100 pixel sur l'image
            int iteration_max = 300;

            int image_x = Convert.ToInt32((BorneDroiteX - BorneGaucheX) * zoom);
            int image_y = Convert.ToInt32((BorneBasseY - BorneHauteY) * zoom);

            RGB[,] Fractale = new RGB[image_x, image_y];

            for (int x = 0; x < image_x; x++)
            {
                for (int y = 0; y < image_y; y++)
                {

                    double NombreReel = 0.285;
                    double NombreComplexe = 0.01;
                    double ComplexePartieR = x / zoom + BorneGaucheX;
                    double ComplexePariteI = y / zoom + BorneHauteY;
                    int i = 0;


                    do
                    {
                        double tmp = ComplexePartieR;
                        ComplexePartieR = ComplexePartieR * ComplexePartieR - ComplexePariteI * ComplexePariteI + NombreReel;
                        ComplexePariteI = 2 * ComplexePariteI * tmp + NombreComplexe;
                        i++;
                    } while (ComplexePartieR * ComplexePartieR + ComplexePariteI * ComplexePariteI < 4 && i < iteration_max);


                    if (i == iteration_max)
                    {

                        Fractale[x, y] = new RGB(0, 0, 0);

                    }
                    else
                    {
                        if (couleur == 1)
                        {
                            Fractale[x, y] = new RGB((int)i * 255 / iteration_max, 0, 0);
                        }
                        if (couleur == 2)
                        {
                            Fractale[x, y] = new RGB(0, (int)i * 255 / iteration_max, 0);
                        }
                        if (couleur == 3)
                        {
                            Fractale[x, y] = new RGB(0, 0, (int)i * 255 / iteration_max);
                        }
                        if (couleur == 4)
                        {
                            Fractale[x, y] = new RGB((int)i * 255 / iteration_max, (int)i * 255 / iteration_max, 0);
                        }
                        if (couleur == 5)
                        {
                            Fractale[x, y] = new RGB((int)i * 255 / iteration_max, 0, (int)i * 255 / iteration_max);
                        }
                        if (couleur == 6)
                        {
                            Fractale[x, y] = new RGB(0, (int)i * 255 / iteration_max, (int)i * 255 / iteration_max);
                        }
                        if (couleur == 7)
                        {
                            Fractale[x, y] = new RGB((int)i * 255 / iteration_max, (int)i * 255 / iteration_max, (int)i * 255 / iteration_max);
                        }
                    }
                }
            }
            hauteur = image_x;
            largeur = image_y;
            tailleFichier = 3 * image_y * image_x + tailleOffset;
            matriceRGB = Fractale;
        }

        #endregion

        #region Matrice de Convolution

        /// <summary>
        /// La matrice de convolution est une matrice qui va pouvoir modifier notre matrice image.
        ///     Grace a elle, nous allons pouvoir faire faires des fonction de filtres generique grace a un noyau.
        ///     Suivant les valeurs donnees dans la matrice noyau, le filtre applique sera different.
        ///     Dans notre fonction, on utilise un nombre pour differencier les differentes fonctions ce qui va pouvoir nous
        ///         autoriser a utiliser les differents filtres correctement.
        /// </summary>
        /// <param name="noyau">Le noyau pris en parametre depends de chaque fonction et est donne par un void separe du code de convolution pour modifier les noyaux si on en a besoin</param>
        /// <param name="nombre">Le nombre est donne en parrallele des noyau et permet de distinguer les differents filtres</param>
        public void Convolution(int[,] noyau, int nombre)
        {
            if (nombre == 1)
            {
                int NoyauX = 0;
                int NoyauY = 0;
                int[] Rouge = new int[this.hauteur * this.largeur];
                int[] Vert = new int[this.hauteur * this.largeur];
                int[] Bleu = new int[this.hauteur * this.largeur];

                for (int i = 0; i < matriceRGB.GetLength(0); i++)
                {
                    Rouge[i] = 0;
                    Vert[i] = 0;
                    Bleu[i] = 0;
                }
                int PositionCouleur = 0;
                for (int i = 1; i < this.matriceRGB.GetLength(0) - 1; i++)
                {
                    for (int j = 1; j < this.matriceRGB.GetLength(1) - 1; j++)
                    {
                        int RougeFinalCouleur = 0;
                        int VertFinalCouleur = 0;
                        int BleuFinalCouleur = 0;

                        for (int a = i - 1; a <= i + 1; a++)
                        {
                            for (int b = j - 1; b <= j + 1; b++)
                            {
                                RougeFinalCouleur += this.matriceRGB[a, b].Rouge * noyau[NoyauX, NoyauY];
                                VertFinalCouleur += this.matriceRGB[a, b].Vert * noyau[NoyauX, NoyauY];
                                BleuFinalCouleur += this.matriceRGB[a, b].Bleu * noyau[NoyauX, NoyauY];

                                NoyauY++;
                            }
                            NoyauX++;
                            NoyauY = 0;
                        }
                        NoyauX = 0;

                        if (RougeFinalCouleur > 255) { RougeFinalCouleur = 255; }
                        if (VertFinalCouleur > 255) { VertFinalCouleur = 255; }
                        if (BleuFinalCouleur > 255) { BleuFinalCouleur = 255; }
                        if (RougeFinalCouleur < 0) { RougeFinalCouleur = 0; }
                        if (VertFinalCouleur < 0) { VertFinalCouleur = 0; }
                        if (BleuFinalCouleur < 0) { BleuFinalCouleur = 0; }

                        Rouge[PositionCouleur] = RougeFinalCouleur;
                        Vert[PositionCouleur] = VertFinalCouleur;
                        Bleu[PositionCouleur] = BleuFinalCouleur;

                        PositionCouleur++;
                    }
                }
                PositionCouleur = 0;

                for (int i = 0; i < matriceRGB.GetLength(0); i++)
                {
                    for (int j = 0; j < matriceRGB.GetLength(1); j++)
                    {
                        if (i == 0 || i == matriceRGB.GetLength(0) - 1 || j == 0 || j == matriceRGB.GetLength(1) - 1)
                        {
                            matriceRGB[i, j].Rouge = 0;
                            matriceRGB[i, j].Vert = 0;
                            matriceRGB[i, j].Bleu = 0;
                        }
                        else
                        {
                            matriceRGB[i, j].Rouge = Rouge[PositionCouleur];
                            matriceRGB[i, j].Vert = Vert[PositionCouleur];
                            matriceRGB[i, j].Bleu = Bleu[PositionCouleur];

                            PositionCouleur++;
                        }
                    }
                }
            }
            if (nombre == 2)
            {
                int NoyauX = 0;
                int NoyauY = 0;
                int[] Rouge = new int[this.hauteur * this.largeur];
                int[] Vert = new int[this.hauteur * this.largeur];
                int[] Bleu = new int[this.hauteur * this.largeur];

                for (int i = 0; i < matriceRGB.GetLength(0); i++)
                {
                    Rouge[i] = 0;
                    Vert[i] = 0;
                    Bleu[i] = 0;
                }
                int PositionCouleur = 0;
                for (int i = 1; i < this.matriceRGB.GetLength(0) - 1; i++)
                {
                    for (int j = 1; j < this.matriceRGB.GetLength(1) - 1; j++)
                    {
                        int RougeFinalCouleur = 0;
                        int VertFinalCouleur = 0;
                        int BleuFinalCouleur = 0;

                        for (int a = i - 1; a <= i + 1; a++)
                        {
                            for (int b = j - 1; b <= j + 1; b++)
                            {
                                RougeFinalCouleur += this.matriceRGB[a, b].Rouge * noyau[NoyauX, NoyauY];
                                VertFinalCouleur += this.matriceRGB[a, b].Vert * noyau[NoyauX, NoyauY];
                                BleuFinalCouleur += this.matriceRGB[a, b].Bleu * noyau[NoyauX, NoyauY];

                                NoyauY++;
                            }
                            NoyauX++;
                            NoyauY = 0;
                        }
                        NoyauX = 0;

                        if (RougeFinalCouleur > 255) { RougeFinalCouleur = 255; }
                        if (VertFinalCouleur > 255) { VertFinalCouleur = 255; }
                        if (BleuFinalCouleur > 255) { BleuFinalCouleur = 255; }
                        if (RougeFinalCouleur < 0) { RougeFinalCouleur = 0; }
                        if (VertFinalCouleur < 0) { VertFinalCouleur = 0; }
                        if (BleuFinalCouleur < 0) { BleuFinalCouleur = 0; }

                        Rouge[PositionCouleur] = RougeFinalCouleur;
                        Vert[PositionCouleur] = VertFinalCouleur;
                        Bleu[PositionCouleur] = BleuFinalCouleur;

                        PositionCouleur++;
                    }
                }
                PositionCouleur = 0;

                for (int i = 0; i < matriceRGB.GetLength(0); i++)
                {
                    for (int j = 0; j < matriceRGB.GetLength(1); j++)
                    {
                        if (i == 0 || i == matriceRGB.GetLength(0) - 1 || j == 0 || j == matriceRGB.GetLength(1) - 1)
                        {
                            matriceRGB[i, j].Rouge = 0;
                            matriceRGB[i, j].Vert = 0;
                            matriceRGB[i, j].Bleu = 0;
                        }
                        else
                        {
                            matriceRGB[i, j].Rouge = Rouge[PositionCouleur];
                            matriceRGB[i, j].Vert = Vert[PositionCouleur];
                            matriceRGB[i, j].Bleu = Bleu[PositionCouleur];

                            PositionCouleur++;
                        }
                    }
                }
            }
            if (nombre == 3)
            {
                int NoyauX = 0;
                int NoyauY = 0;

                for (int i = 1; i < this.matriceRGB.GetLength(0) - 1; i++)
                {
                    for (int j = 1; j < this.matriceRGB.GetLength(1) - 1; j++)
                    {
                        int RougeFinalCouleur = 0;
                        int VertFinalCouleur = 0;
                        int BleuFinalCouleur = 0;
                        for (int a = i - 1; a <= i + 1; a++)
                        {
                            for (int b = j - 1; b <= j + 1; b++)
                            {
                                RougeFinalCouleur += this.matriceRGB[a, b].Rouge * noyau[NoyauX, NoyauY];
                                VertFinalCouleur += this.matriceRGB[a, b].Vert * noyau[NoyauX, NoyauY];
                                BleuFinalCouleur += this.matriceRGB[a, b].Bleu * noyau[NoyauX, NoyauY];
                                NoyauY++;
                            }
                            NoyauX++;
                            NoyauY = 0;
                        }
                        NoyauX = 0;

                        matriceRGB[i, j].Rouge = RougeFinalCouleur / 9;
                        matriceRGB[i, j].Vert = VertFinalCouleur / 9;
                        matriceRGB[i, j].Bleu = BleuFinalCouleur / 9;
                    }
                }
            }
            if (nombre == 4)
            {
                int NoyauX = 0;
                int NoyauY = 0;
                int[] Rouge = new int[this.hauteur * this.largeur];
                int[] Vert = new int[this.hauteur * this.largeur];
                int[] Bleu = new int[this.hauteur * this.largeur];
                for (int i = 0; i < matriceRGB.GetLength(0); i++)
                {
                    Rouge[i] = 0;
                    Vert[i] = 0;
                    Bleu[i] = 0;
                }
                int PositionCouleur = 0;
                for (int i = 1; i < this.matriceRGB.GetLength(0) - 1; i++)
                {
                    for (int j = 1; j < this.matriceRGB.GetLength(1) - 1; j++)
                    {
                        int RougeFinalCouleur = 0;
                        int VertFinalCouleur = 0;
                        int BleuFinalCouleur = 0;
                        for (int a = i - 1; a <= i + 1; a++)
                        {
                            for (int b = j - 1; b <= j + 1; b++)
                            {
                                RougeFinalCouleur += this.matriceRGB[a, b].Rouge * noyau[NoyauX, NoyauY];
                                VertFinalCouleur += this.matriceRGB[a, b].Vert * noyau[NoyauX, NoyauY];
                                BleuFinalCouleur += this.matriceRGB[a, b].Bleu * noyau[NoyauX, NoyauY];
                                NoyauY++;
                            }
                            NoyauX++;
                            NoyauY = 0;
                        }
                        NoyauX = 0;

                        if (RougeFinalCouleur > 255) { RougeFinalCouleur = 255; }
                        if (VertFinalCouleur > 255) { VertFinalCouleur = 255; }
                        if (BleuFinalCouleur > 255) { BleuFinalCouleur = 255; }
                        if (RougeFinalCouleur < 0) { RougeFinalCouleur = 0; }
                        if (VertFinalCouleur < 0) { VertFinalCouleur = 0; }
                        if (BleuFinalCouleur < 0) { BleuFinalCouleur = 0; }

                        Rouge[PositionCouleur] = RougeFinalCouleur;
                        Vert[PositionCouleur] = VertFinalCouleur;
                        Bleu[PositionCouleur] = BleuFinalCouleur;

                        PositionCouleur++;
                    }
                }
                PositionCouleur = 0;

                for (int i = 1; i < matriceRGB.GetLength(0) - 1; i++)
                {
                    for (int j = 1; j < matriceRGB.GetLength(1) - 1; j++)
                    {
                        matriceRGB[i, j].Rouge = Rouge[PositionCouleur];
                        matriceRGB[i, j].Vert = Vert[PositionCouleur];
                        matriceRGB[i, j].Bleu = Bleu[PositionCouleur];

                        PositionCouleur++;
                    }
                }
            }
        }

        /// <summary>
        /// Donne le noyau pour le filtre "Detection des contours" et le nombre pour la fonction de Convolution.
        /// </summary>
        public void Contours()
        {
            int[,] noyau = { { 0, 1, 0 }, { 1, -4, 1 }, { 0, 1, 0 } };
            Convolution(noyau, 1);
        }

        /// <summary>
        /// Donne le noyau pour le filtre de "Renforcement des bords" et le nombre pour la fonction de Convolution,
        /// </summary>
        public void Renforcement_Bords()
        {
            int[,] noyau = { { 0, 0, 0 }, { -1, 1, 0 }, { 0, 0, 0 } };
            Convolution(noyau, 2);
        }

        /// <summary>
        /// Donne le noyau pour le filtre de "Flouter" et le nombre pour la fonction de Convolution.
        /// </summary>
        public void Flou()
        {
            int[,] noyau = { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };
            Convolution(noyau, 3);
        }

        /// <summary>
        /// Donne le noyau pour le filtre de "Repoussage" et le nombre pour la fonction de Convolution
        /// </summary>
        public void Repoussage()
        {
            int[,] noyau = new int[,] { { -2, -1, 0 }, { -1, 1, 1 }, { 0, 1, 2 } };
            Convolution(noyau, 4);
        }

        #endregion

        #region QR Code

        public void QRCode()
        {
            //tableau de caractères alphanumériques
            char[] caracteres = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', ' ', '$', '%', '*', '+', '-', '.', '/', ':' };
            bool est_alphanumerique = false;
            string texte = null;
            do
            {
                Console.WriteLine("Quel texte voulez vous générer en code QR ?\n Attention: Vous avez une limite de 47 svp"); //affichage
                texte = Console.ReadLine();
                texte = texte.ToUpper();
                int compteur = 0;
                for (int i = 0; i < texte.Length; i++) //parcourir le tableau aplhanumérique afin de vérifieer si les caractères existent
                {
                    for (int j = 0; j < caracteres.Length; j++)
                    {
                        if (texte[i] == caracteres[j])
                        {
                            compteur++;
                        }
                    }
                }
                if (compteur == texte.Length) est_alphanumerique = true;
            } while (est_alphanumerique == false);


            string chaine = "0010"; //premiers bits de la chaine : mode
            string tailleTexte = Convert.ToString(texte.Length, 2); //convertit la taille en binaire
            if (tailleTexte.Length < 9) //ajoute des 0 si la chaine ne fait pas 9 bits
            {
                for (int i = 0; i < 9 - tailleTexte.Length; i++)
                {
                    chaine += "0";
                }
            }
            chaine += tailleTexte;

            string lettre1 = null;
            string lettre2 = null;
            string couple = null;
            string bitsCouple = null;
            string donnees = null;
            string dernierChiffre = null;
            string bitsDernier = null;
            for (int i = 0; i < texte.Length; i += 2) //convertit en binaire les caractères alphanumériques par couple
            {
                if (i != texte.Length - 1)
                {
                    for (int j = 0; j < caracteres.Length; j++)
                    {
                        if (texte[i] == caracteres[j])
                        {
                            lettre1 = Convert.ToString(j * 45);
                        }
                        if (texte[i + 1] == caracteres[j])
                        {
                            lettre2 = Convert.ToString(j);
                        }

                    }

                    int somme = Convert.ToInt32(lettre1) + Convert.ToInt32(lettre2);
                    couple = Convert.ToString(somme, 2);

                    if (couple.Length < 11) //ajoute des 0 si la chaine ne fait pas 11 bits
                    {
                        for (int l = 0; l < 11 - couple.Length; l++)
                        {
                            bitsCouple += "0";
                        }
                    }
                    bitsCouple += couple;
                }

                else
                {
                    for (int j = 0; j < caracteres.Length; j++)
                    {
                        if (texte[i] == caracteres[j])
                        {
                            lettre1 = Convert.ToString(j);
                        }
                    }

                    dernierChiffre = Convert.ToString(Convert.ToInt32(lettre1), 2);

                    if (dernierChiffre.Length < 6) //on ajoute des 0 si le dernier caractère converti en binaire ne fait pas 6 bits
                    {
                        for (int l = 0; l < 6 - dernierChiffre.Length; l++)
                        {
                            bitsDernier += "0";
                        }
                    }
                    bitsDernier += dernierChiffre;
                }
            }
            donnees += bitsCouple;
            donnees += bitsDernier;
            chaine += donnees; //ajout des donnéesbinaires à la chaine existante


            int nbBits = 0;
            int version = 0;
            //determination des versions
            if (texte.Length <= 25)
            {
                nbBits = 152;
                version = 1;
            }
            else
            {
                nbBits = 272;
                version = 2;
            }

            if ((nbBits - chaine.Length) <= 4)//ajoute jusqu'à 4 0 si la chaine est trop courte
            {
                for (int i = 0; i < (nbBits - chaine.Length); i++)
                {
                    chaine += "0";
                }
            }
            else chaine += "0000";

            do
            {
                chaine += "0";
            } while (chaine.Length % 8 != 0);//ajoute des 0 jusqu'à que le nombre de caractères de la chaine soit un multiple de 8

            while (chaine.Length < nbBits)//ajoute des octets spécifiques jusqu'à que la taille requise soit atteinte
            {
                chaine += "11101100";
                if (chaine.Length < nbBits)
                {
                    chaine += "00010001";
                }
            }

            byte Convertisseur_Binaire(string b) //convertit un suite de bits en un octet base 10
            {
                byte resultat = 0;
                for (int i = b.Length - 1; i >= 0; i--)
                {
                    if (b[i] == '1')
                    {
                        if (i == b.Length - 1)
                        {
                            resultat += 1;
                        }
                        else resultat += (byte)Math.Pow(2, b.Length - 1 - i);
                    }
                }
                return resultat;
            }

            string aConvertir = null;
            int index = 0;
            //création d'un tableau d'octets à partir de la chaine de bits pour la correction Reed Solomon
            byte[] octets = new byte[(chaine.Length / 8) + 1];
            for (int i = 0; i < chaine.Length; i += 8)
            {
                aConvertir = null;
                for (int j = 0; j < 8; j++)
                {
                    aConvertir += chaine[i + j];
                }
                index++;
                octets[index] = Convertisseur_Binaire(aConvertir);
            }

            //Correction Reed Solomon
            int n = 0;
            if (version == 1)
            {
                n = 7;
            }
            else n = 10;
            byte[] reedSolomon_byte = ReedSolomonAlgorithm.Encode(octets, n, ErrorCorrectionCodeType.QRCode);

            //reconversion en chaine binaire
            string reedSolomon_binaire = null;
            for (int i = 0; i < reedSolomon_byte.Length; i++)
            {
                if (Convert.ToString(Convert.ToInt32(reedSolomon_byte[i]), 2).Length < 8)
                {
                    for (int j = 0; j < 8 - (Convert.ToString(Convert.ToInt32(reedSolomon_byte[i]), 2).Length); j++)
                    {
                        reedSolomon_binaire += "0";
                    }
                    reedSolomon_binaire += Convert.ToString(Convert.ToInt32(reedSolomon_byte[i]), 2);
                }
                else reedSolomon_binaire += Convert.ToString(Convert.ToInt32(reedSolomon_byte[i]), 2);
            }

            chaine += reedSolomon_binaire;
            //Console.WriteLine(chaine);

            //création de la matrice pour qr code
            byte[,] matriceQR = new byte[(version - 1) * 4 + 21, ((version - 1) * 4 + 21)];
            //initialisation des cases à 5
            for (int i = 0; i < matriceQR.GetLength(0); i++)
            {
                for (int j = 0; j < matriceQR.GetLength(1); j++)
                {
                    matriceQR[i, j] = 5;
                }
            }
            //finder patterns

            //pattern en haut à gauche
            for (int i = 0; i < 7; i++)
            {
                //couche externe noire : on met à 1 touts les modules qui seront noirs
                matriceQR[0, i] = 1;
                matriceQR[i, 0] = 1;
                matriceQR[i, 6] = 1;
                matriceQR[6, i] = 1;

                //couche blanche intérieure : on met à 0 touts les modules qui seront blancs
                for (int j = 1; j < 6; j++)
                {
                    matriceQR[1, j] = 0;
                    matriceQR[j, 1] = 0;
                    matriceQR[j, 5] = 0;
                    matriceQR[5, j] = 0;
                }

                //intérieur noir : on met à 1 touts les modules qui forment le carré internieur noir
                for (int j = 2; j < 5; j++)
                {
                    matriceQR[2, j] = 1;
                    matriceQR[4, j] = 1;
                    matriceQR[3, j] = 1;
                }

            }

            //pattern haut droite
            int point_hautDroite = ((version - 1) * 4 + 21) - 7;
            for (int i = 0; i < 7; i++)
            {
                //couche externe noire
                matriceQR[0, point_hautDroite + i] = 1;
                matriceQR[i, point_hautDroite] = 1;
                matriceQR[i, matriceQR.GetLength(1) - 1] = 1;
                matriceQR[6, point_hautDroite + i] = 1;

                //couche blanche intérieure
                for (int j = 1; j < 6; j++)
                {
                    matriceQR[1, point_hautDroite + j] = 0;
                    matriceQR[j, point_hautDroite + 1] = 0;
                    matriceQR[j, matriceQR.GetLength(1) - 2] = 0;
                    matriceQR[5, point_hautDroite + j] = 0;
                }

                //intérieur noir
                for (int j = 2; j < 5; j++)
                {
                    matriceQR[2, point_hautDroite + j] = 1;
                    matriceQR[4, point_hautDroite + j] = 1;
                    matriceQR[3, point_hautDroite + j] = 1;
                }

            }

            //pattern bas gauche
            int point_basGauche = ((version - 1) * 4 + 21) - 7;
            for (int i = 0; i < 7; i++)
            {
                //couche externe noire
                matriceQR[point_basGauche, i] = 1;
                matriceQR[point_basGauche + i, 0] = 1;
                matriceQR[point_basGauche + i, 6] = 1;
                matriceQR[matriceQR.GetLength(0) - 1, i] = 1;

                //couche blanche intérieure
                for (int j = 1; j < 6; j++)
                {
                    matriceQR[point_basGauche + 1, j] = 0;
                    matriceQR[point_basGauche + j, 1] = 0;
                    matriceQR[point_basGauche + j, 5] = 0;
                    matriceQR[matriceQR.GetLength(0) - 2, j] = 0;
                }

                //intérieur noir
                for (int j = 2; j < 5; j++)
                {
                    matriceQR[point_basGauche + 2, j] = 1;
                    matriceQR[point_basGauche + 4, j] = 1;
                    matriceQR[point_basGauche + 3, j] = 1;
                }
            }

            //séparateurs
            //séparateur haut gauche : on met à 0 les modules suivants pour créer les lignes blanches
            for (int i = 0; i < 8; i++)
            {
                matriceQR[i, 7] = 0;
                matriceQR[7, i] = 0;
            }

            //séparateur haut droit
            for (int i = 0; i < 8; i++)
            {
                matriceQR[i, point_hautDroite - 1] = 0;
                matriceQR[7, point_hautDroite - 1 + i] = 0;
            }

            //séparateur bas gauche
            for (int i = 0; i < 8; i++)
            {
                matriceQR[point_basGauche - 1, i] = 0;
                matriceQR[point_basGauche - 1 + i, 7] = 0;
            }

            //patterns d'alignement
            if (version == 2)
            {
                matriceQR[18, 18] = 1; //centre noir
                for (int i = 0; i < 3; i++)
                {
                    //contour blanc
                    matriceQR[17, 17 + i] = 0;
                    matriceQR[19, 17 + i] = 0;
                    matriceQR[18, 17] = 0;
                    matriceQR[18, 19] = 0;
                }
                for (int i = 0; i < 5; i++)
                {
                    //contour noir
                    matriceQR[16, 16 + i] = 1;
                    matriceQR[16 + i, 16] = 1;
                    matriceQR[16 + i, 20] = 1;
                    matriceQR[20, 16 + i] = 1;
                }
            }

            //timing patterns
            //ligne verticale
            for (int i = 8; i < matriceQR.GetLength(0) - 8; i += 2)
            {
                matriceQR[i, 6] = 1;
            }
            for (int i = 9; i < matriceQR.GetLength(0) - 9; i += 2)
            {
                matriceQR[i, 6] = 0;
            }
            //ligne horizontale
            for (int i = 8; i < matriceQR.GetLength(1) - 8; i += 2)
            {
                matriceQR[6, i] = 1;
            }
            for (int i = 9; i < matriceQR.GetLength(1) - 9; i += 2)
            {
                matriceQR[6, i] = 0;
            }

            //placement du dark module
            matriceQR[(4 * version) + 9, 8] = 1;

            //modules réservés au format
            //en haut à gauche
            for (int i = 0; i < 9; i++)
            {
                if (matriceQR[8, i] == 5 && matriceQR[i, 8] == 5)
                {
                    matriceQR[8, i] = 2;
                    matriceQR[i, 8] = 2;
                }
            }
            //en haut à droite
            for (int i = 0; i < 8; i++)
            {
                matriceQR[8, point_hautDroite - 1 + i] = 2;
            }
            //en bas à gauche
            for (int i = 0; i < 7; i++)
            {
                matriceQR[(4 * version + 10) + i, 8] = 2;
            }

            //ajout des données
            bool up = true;
            int m = 0;
            byte[] chaineNb = new byte[chaine.Length];
            for (int i = 0; i < chaine.Length; i++)
            {
                if (chaine[i] == '0')
                {
                    chaineNb[i] = 0;
                }
                else chaineNb[i] = 1;
                //Console.Write(chaineNb[i]);

            }
            for (int b = matriceQR.GetLength(0) - 1; b >= 0; b -= 2)
            {
                if (b == 6) //saut de la colonne 6
                {
                    b--;
                }
                if (up == true)
                {
                    for (int a = matriceQR.GetLength(1) - 1; a >= 0 && m < chaineNb.Length - 1; a--)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            if (matriceQR[a, b - i] == 5)
                            {

                                matriceQR[a, b - i] = chaineNb[m];

                                if ((a + b - i) % 2 == 0)
                                {
                                    if (matriceQR[a, b - i] == 0)
                                    {
                                        matriceQR[a, b - i] = 1;
                                    }
                                    else if (matriceQR[a, b - i] == 1)
                                    {
                                        matriceQR[a, b - i] = 0;
                                    }
                                }
                                m++;
                            }

                        }

                    }
                    up = false;
                }
                else
                {
                    for (int a = 0; a < matriceQR.GetLength(0) && m < chaine.Length - 1; a++)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            if (matriceQR[a, b - i] == 5)
                            {
                                matriceQR[a, b - i] = chaineNb[m];

                                if ((a + b - i) % 2 == 0)
                                {
                                    {
                                        if (matriceQR[a, b - i] == 0)
                                        {
                                            matriceQR[a, b - i] = 1;
                                        }
                                        else if (matriceQR[a, b - i] == 1)
                                        {
                                            matriceQR[a, b - i] = 0;
                                        }
                                    }

                                }
                                m++;
                            }
                        }
                    }
                    up = true;
                }

            }

            //placement des bits de version
            //haut gauche horizontal
            byte[] bitsVersion = new byte[15] { 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 0, 0, 1, 0, 0 };
            int cpt = 0;
            for (int i = 0; i < 9; i++)
            {
                if (matriceQR[8, i] == 2)
                {
                    matriceQR[8, i] = bitsVersion[cpt];
                    cpt++;
                }
            }
            //haut gauche vertical
            for (int i = 7; i >= 0; i--)
            {
                if (matriceQR[i, 8] == 2)
                {
                    matriceQR[i, 8] = bitsVersion[cpt];
                    cpt++;
                }
            }
            //bas gauche vertical
            int cpt_bis = 0;
            for (int i = matriceQR.GetLength(0) - 1; i >= matriceQR.GetLength(0) - 7; i--)
            {
                matriceQR[i, 8] = bitsVersion[cpt_bis];
                cpt_bis++;
            }
            //haut droite horizontal
            for (int i = 0; i < 8; i++)
            {
                if (matriceQR[8, point_hautDroite - 1 + i] == 2)
                {
                    matriceQR[8, point_hautDroite - 1 + i] = bitsVersion[cpt_bis];
                    cpt_bis++;
                }
            }



            //affichage sous forme d'image bitmap
            hauteur = matriceQR.GetLength(0);
            largeur = matriceQR.GetLength(1);
            matriceRGB = new RGB[hauteur, largeur];
            for (int i = hauteur - 1; i >= 0; i--)
            {
                for (int j = 0; j < largeur; j++)
                {
                    if (matriceQR[i, j] == 0)
                    {
                        matriceRGB[hauteur - i - 1, j] = new RGB(255, 255, 255);
                    }
                    if (matriceQR[i, j] == 1)
                    {
                        matriceRGB[hauteur - i - 1, j] = new RGB(0, 0, 0);
                    }
                    if (matriceQR[i, j] == 5)
                    {
                        matriceRGB[hauteur - i - 1, j] = new RGB(255, 255, 255);
                    }
                    if (matriceQR[i, j] == 2)
                    {
                        matriceRGB[hauteur - i - 1, j] = new RGB(255, 0, 0);
                    }
                }
            }
        }

        #endregion
    }
}