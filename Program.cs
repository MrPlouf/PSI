using System;
using System.Diagnostics;

namespace Probleme_Scientifique
{
    class MainClass
    {
        public static void Main(string[] args)
        {

            string nomfichier = null;
            int nombrefichier = -1;
            //MyImage Image = null;
            Console.WriteLine("Qu'elle image voulez vous traiter ?\n");
            Console.WriteLine("1. Coco \n2. Lena \n3. Lac en Montagne \n4. Test \n5. Une image de votre choix \n6. Fractale de Mandelbrot \n7. Fractale (Ensemble) de Julia\n8. Cacher Image dans une autre \n9. QR Code");
            while (nombrefichier != 1 && nombrefichier != 2 && nombrefichier != 3 && nombrefichier != 4 && nombrefichier != 5 && nombrefichier != 6 && nombrefichier != 7 && nombrefichier != 8 && nombrefichier != 9)
            {
                Console.WriteLine("\nDonnez un fichier que vous voulez traiter:");
                nombrefichier = Convert.ToInt32(Console.ReadLine());
            }
            switch (nombrefichier)
            {
                case 1:
                    nomfichier = "coco.bmp";
                    break;

                case 2:
                    nomfichier = "lena.bmp";
                    break;

                case 3:
                    nomfichier = "lac.bmp";
                    break;

                case 4:
                    nomfichier = "test.bmp";
                    break;

                case 5:
                    Console.WriteLine("Donner le nom de votre fichier:");
                    string nomdenotrefichier = Convert.ToString(Console.ReadLine());
                    nomfichier = nomdenotrefichier + ".bmp";
                    break;
            }
            if (nombrefichier == 6)
            {
                Console.Clear();
                MyImage Fractale = new MyImage();
                Console.WriteLine("Quelle precision vouslez vous ? \n(Nombre de pixels de large et de hauteur de l'image finale: on recommande entre 1000 et 3300 pour ne pas crash)\n cf: plus le nombre est grand, plus elle met de temps a charger");
                double zoom = Convert.ToInt32(Console.ReadLine());
                Console.Clear();
                Console.WriteLine("De quelle couleur voulez vous votre Fractale");
                Console.WriteLine("1. Bleu \n2. Vert \n3. Rouge \n4. Cyan \n5. Magenta \n6. Jaune \n7. Blanc \n8. Special Noir et Blanc\n");
                int couleur = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Veuillez patienter...");
                Fractale.FractaleMandelbrot(zoom, couleur);
                Fractale.Rotation(90);
                Fractale.From_Image_To_File("Fractale.bmp");
                Process.Start("Fractale.bmp");
                Environment.Exit(0);
            }
            if (nombrefichier == 7)
            {
                Console.Clear();
                MyImage Fractale1 = new MyImage();
                Console.WriteLine("Quelle precision vouslez vous ? \n(Nombre de pixels de large et de hauteur de l'image finale: on recommande entre 1000 et 3300 pour ne pas crash)\n cf: plus le nombre est grand, plus elle met de temps a charger");
                double zoom1 = Convert.ToInt32(Console.ReadLine());
                Console.Clear();
                Console.WriteLine("De quelle couleur voulez vous votre Fractale");
                Console.WriteLine("1. Bleu \n2. Vert \n3. Rouge \n4. Cyan \n5. Magenta \n6. Jaune \n7. Blanc \n");
                int couleur = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Veuillez patienter...");
                Fractale1.FractaledeJulia(zoom1, couleur);
                Fractale1.Rotation(90);
                Fractale1.From_Image_To_File("Fractale_.bmp");
                Process.Start("Fractale_.bmp");
                Environment.Exit(0);
            }
            if (nombrefichier == 8)
            {
                Main_CacherImage();
            }
            if (nombrefichier == 9)
            {
                Console.Clear();
                MyImage QRcode = new MyImage();
                QRcode.QRCode();
                QRcode.Agrandir(20);
                QRcode.From_Image_To_File("qrcode.bmp");
                Process.Start("qrcode.bmp");
                Console.ReadKey();

            }

            if (nombrefichier != 8 && nombrefichier != 7 && nombrefichier != 6 && nombrefichier != 9)
            {

                MyImage NouvelleImage = new MyImage(nomfichier);
                //NouvelleImage.From_Image_To_File(nomfichier);

                Console.Clear();

                Console.WriteLine("Le fichier choisi est: " + nomfichier + "\n");
                Console.WriteLine("Quelle action voulez-vous faire ?\n Choisissez parmi les suivantes:\n");
                Console.WriteLine(" 1. Passer l'image en nuances de gris");
                Console.WriteLine(" 2. Passer l'image en noir et blanc");
                Console.WriteLine(" 3. Agrandir l'image (par un coefficiant)");
                Console.WriteLine(" 4. Retrecir l'image (par un coefficiant)");
                Console.WriteLine(" 5. Rotation de l'image (par un angle quelconque)");
                Console.WriteLine(" 6. Retourner l'image (en Miroir)");
                Console.WriteLine(" 7. Détecter les contours de l'image");
                Console.WriteLine(" 8. Renforcer les bords de l'image");
                Console.WriteLine(" 9. Flouter l'image (par un coefficiant)");
                Console.WriteLine("10. Repoussage de l'image");
                Console.WriteLine("11. Donner un histogramme de l'image");
                Console.WriteLine("12. Innovation");
                Console.WriteLine("\n Numéro de votre choix: ");

                int n = Convert.ToInt32(Console.ReadLine());
                switch (n)
                {
                    case 1:
                        NouvelleImage.Monochrome_Gris();
                        NouvelleImage.From_Image_To_File("Gris_Sortie.bmp");
                        Process.Start("Gris_Sortie.bmp");
                        break;
                    case 2:
                        NouvelleImage.Noir_Et_Blanc();
                        NouvelleImage.From_Image_To_File("NoirBlanc_Sortie.bmp");
                        Process.Start("NoirBlanc_Sortie.bmp");
                        break;
                    case 3:
                        Console.WriteLine("Un agrandissement de combien ?");
                        int coefficiant_agr = Convert.ToInt32(Console.ReadLine());
                        NouvelleImage.Agrandir(coefficiant_agr);
                        NouvelleImage.From_Image_To_File("Agrandissement_Sortie.bmp");
                        Process.Start("Agrandissement_Sortie.bmp");
                        break;
                    case 4:
                        Console.WriteLine("Un retrecissement de combien ?");
                        int coefficiant_ret = Convert.ToInt32(Console.ReadLine());
                        NouvelleImage.Retrecir(coefficiant_ret);
                        NouvelleImage.From_Image_To_File("Retrecissement_Sortie.bmp");
                        Process.Start("Retrecissement_Sortie.bmp");
                        break;
                    case 5:
                        Console.WriteLine("Quel angle de rotation voulez vous utiliser pour tourner limage ?");
                        int anglerot = Convert.ToInt32(Console.ReadLine());

                        NouvelleImage.Rotation(anglerot);
                        NouvelleImage.From_Image_To_File("Rotation_Sortie.bmp");
                        Process.Start("Rotation_Sortie.bmp");
                        break;
                    case 6:
                        NouvelleImage.Miroir();
                        NouvelleImage.From_Image_To_File("Miroir_Sortie.bmp");
                        Process.Start("Miroir_Sortie.bmp");
                        break;
                    case 7:
                        NouvelleImage.Contours();
                        NouvelleImage.From_Image_To_File("Contours_Sortie.bmp");
                        Process.Start("Contours_Sortie.bmp");
                        break;
                    case 8:
                        NouvelleImage.Renforcement_Bords();
                        NouvelleImage.From_Image_To_File("Renforcements_Sortie.bmp");
                        Process.Start("Renforcements_Sortie.bmp");
                        break;
                    case 9:
                        Console.WriteLine("Combien de fois voulez vous flouter l'image ?");
                        int multiplicateur = Convert.ToInt32(Console.ReadLine());
                        for (int i = 0; i < multiplicateur; i++)
                        {
                            NouvelleImage.Flou();
                        }
                        NouvelleImage.From_Image_To_File("Flou_sortie.bmp");
                        Process.Start("Flou_Sortie.bmp");
                        break;
                    case 10:
                        NouvelleImage.Repoussage();
                        NouvelleImage.From_Image_To_File("Repoussage_sortie.bmp");
                        Process.Start("Repoussage_Sortie.bmp");
                        break;
                    case 11:
                        NouvelleImage.Histogramme();
                        NouvelleImage.From_Image_To_File("Histogramme.bmp");
                        Process.Start("Histogramme.bmp");
                        break;
                    case 12:
                        NouvelleImage.Innovation();
                        NouvelleImage.From_Image_To_File("Innovation.bmp");
                        Process.Start("Innovation.bmp");
                        break;

                    default:
                        Console.ReadLine();
                        break;
                }

                Console.ReadKey();
            }
        }

        public static void Main_CacherImage()
        {
            Console.Clear();
            Console.WriteLine("Donner l'image que vous voulez cacher:\n");
            Console.WriteLine("1. Coco \n2. Lena \n3. Lac en Montagne \n4. Test \n5. Une image de votre choix");
            int reponse1 = -1;
            string reponsefichier0 = null;
            while (reponse1 != 1 && reponse1 != 2 && reponse1 != 3 && reponse1 != 4 && reponse1 != 5)
            {
                Console.WriteLine("\nDonnez un fichier que vous voulez traiter:");
                reponse1 = Convert.ToInt32(Console.ReadLine());
            }
            switch (reponse1)
            {
                case 1:
                    reponsefichier0 = "coco.bmp";
                    break;

                case 2:
                    reponsefichier0 = "lena.bmp";
                    break;

                case 3:
                    reponsefichier0 = "lac.bmp";
                    break;

                case 4:
                    reponsefichier0 = "test.bmp";
                    break;

                case 5:
                    Console.WriteLine("Donner le nom de votre fichier:");
                    string reponsefichier = Convert.ToString(Console.ReadLine());
                    reponsefichier0 = reponsefichier + ".bmp";
                    break;
            }
            Console.Clear();
            Console.WriteLine("Image cachee: " + reponsefichier0);
            Console.WriteLine("Donner l'image dans laquelle vous vouler la cacher:\n");
            Console.WriteLine("1. Coco \n2. Lena \n3. Lac en Montagne \n4. Test \n5. Une image de votre choix");
            int reponse2 = -1;
            string reponsefichier1 = null;
            while (reponse2 != 1 && reponse2 != 2 && reponse2 != 3 && reponse2 != 4 && reponse2 != 5)
            {
                Console.WriteLine("\nDonnez un fichier que vous voulez traiter:");
                reponse2 = Convert.ToInt32(Console.ReadLine());
            }
            switch (reponse2)
            {
                case 1:
                    reponsefichier1 = "coco.bmp";
                    break;

                case 2:
                    reponsefichier1 = "lena.bmp";
                    break;

                case 3:
                    reponsefichier1 = "lac.bmp";
                    break;

                case 4:
                    reponsefichier1 = "test.bmp";
                    break;

                case 5:
                    Console.WriteLine("Donner le nom de votre fichier:");
                    string reponsefichier9 = Convert.ToString(Console.ReadLine());
                    reponsefichier1 = reponsefichier9 + ".bmp";
                    break;
            }
            MyImage ImageSupport = new MyImage(reponsefichier1);
            MyImage ImageCachee = new MyImage(reponsefichier0);

            ImageSupport.CacherImageDansAutre(ImageCachee);
            ImageSupport.From_Image_To_File("Sortie_Cachee.bmp");
            Process.Start("Sortie_Cachee.bmp");
            Console.Clear();
            Console.WriteLine("Voulez-vous retrouver votre image cachee ?");
            Console.WriteLine("1.Oui \n2.Non");
            int reponse = Convert.ToInt32(Console.ReadLine());
            switch (reponse)
            {
                case 1:
                    Console.Clear();
                    ImageSupport.TrouverImageCachee(ImageCachee);
                    ImageSupport.From_Image_To_File("Sortie_Retrouver.bmp");
                    Process.Start("Sortie_Retrouver.bmp");
                    break;

                case 2:
                    Console.ReadKey();
                    break;
            }

        }

    }
}
