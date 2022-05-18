namespace Probleme_Scientifique
{
    public class RGB
    {
        private int bleu;
        private int vert;
        private int rouge;

        public RGB(int B, int G, int R)
        {
            rouge = R;
            vert = G;
            bleu = B;
        }

        public RGB(int[] RGB)
        {
            this.Rouge = RGB[0];
            this.Vert = RGB[1];
            this.Bleu = RGB[2];
        }
        public RGB(RGB rgb)
        {
            Rouge = rgb.Rouge;
            Vert = rgb.Vert;
            Bleu = rgb.Bleu;
        }
        public int Rouge
        {
            get { return rouge; }
            set { rouge = value; }
        }

        public int Vert
        {
            get { return vert; }
            set { vert = value; }
        }

        public int Bleu
        {
            get { return bleu; }
            set { bleu = value; }
        }

        /// <summary>
        /// Cette fonction donne la valeur grise du pixel en faisant la moyenne des valeurs de rouge, vert et bleu.
        /// </summary>
        public void Gris()
        {
            int gris = (this.rouge + this.vert + this.bleu) / 3;
            this.rouge = gris;
            this.vert = gris;
            this.bleu = gris;
        }
    }


}
