var tablica_danych = [[1,2,3],[4,5,6],[7,8,9]];

function generate_file(tablica)
{
    let tresc_pliku = "P2\n";
    tresc_pliku += tablica.length + " " + tablica[0].length + "\n";
    let max = 0; 
    for(wiersz of tablica)
    {
        for(wartosc of wiersz)
        {
            if(wartosc > max)
                max = wartosc;
        }
    }
    tresc_pliku += max + "\n";
    let pierwszy_wiersz = true;
    for(wiersz of tablica)
    {
        if(!pierwszy_wiersz)
        {
            tresc_pliku += "\n";
        }
        else
            pierwszy_wiersz = false;
        let pierwsza_dana = true;
        for(wartosc of wiersz)
        {
            if(!pierwsza_dana)
            {
                tresc_pliku += " ";
            }
            else
                pierwsza_dana = false;

            tresc_pliku += wartosc;
        }
    }
        console.log(tresc_pliku);
        var filename = "wygenerowany_plik";
        var blob = new Blob([tresc_pliku], {type: "text/plain;charset=utf-8"});
        saveAs(blob, filename+".pgm");
}

generate_file(tablica_danych);