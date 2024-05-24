# To jest tytul.
## Header To jest tytul. dalsza część

jakis tekst
Dalszy y=tekst 123.
*hejka lubie*
**hejka lubie**

[Kliknij](https://gars.pl)



```Cpp

# code here
#include <algorithm>
#include <cstring>
#include <utility> // std::exchange
using namespace std;

#include "simpleString.h"

#ifndef _MSC_FULL_VER // if not Visual Studio Compiler
    #warning "Klasa jest do zaimplementowania. Instrukcja w pliku naglowkowym"
#else
    #pragma message ("Klasa jest do zaimplementowania. Instrukcja w pliku naglowkowym")
#endif


size_t SimpleString::instances_ = 0;

SimpleString::SimpleString() {
    instances_ ++;
    data_ = new char[1]{};  //wypelnia sie zerami
}

SimpleString::SimpleString(const char *text) {
    instances_ ++;
    data_ = new char[strlen(text)+1];
    strcpy(data_, text);
    size_ = strlen(data_);
    capacity_ = size_;
}

SimpleString::SimpleString(const SimpleString &text) {
    instances_ ++;
    data_ = new char[text.size()+1];
    strcpy(data_, text.data_);
    size_ = strlen(data_);
    capacity_ = size_;
}

SimpleString::~SimpleString() {
    instances_ --;
    delete[] data_;
}

SimpleString::SimpleString(SimpleString&& string){
    instances_++;
    data_ = new char[string.size()+1];
    strcpy(data_, string.data_);
    size_ = strlen(data_);
    capacity_ = size_;
    *string.data_ = '\0';
    string.size_ = 0;
    string.capacity_ = 0;
}


size_t SimpleString::size() const {
    return size_;
}

size_t SimpleString::capacity() const {
    return capacity_;
}

const char* SimpleString::data() const {
    return data_;
}

const char* SimpleString::c_str() const {
    return data_;
}

size_t SimpleString::instances() {
    return instances_;
}

void SimpleString::assign(const char *new_text) {
    if(capacity_ >= strlen(new_text)) strcpy(data_, new_text);
    else{
        delete[] data_;
        data_ = new char[strlen(new_text)+1];
        strcpy(data_, new_text);
        capacity_ = strlen(data_);
    }
    size_ = strlen(data_);
}

bool SimpleString::equal_to(SimpleString string2, bool case_sensitive) const{
    if(case_sensitive){
        if(!strcmp(data_, string2.data_)) return true;
        return false;
    }
    char *s1 = data_;
    for(;*s1;s1++) if(*s1 >= 'A' && *s1 <= 'Z') *s1 -= 'A'-'a';//*s1 = tolower(*s1)
    char *s2 = (char*) string2.data_;
    for(;*s2;s2++) if(*s2 >= 'A' && *s2 <= 'Z') *s2 -= 'A'-'a';
    if(*s1 == *s2) return true;
    return false;
}

void SimpleString::append(SimpleString string2) {
    if(capacity_ >= size() + string2.size()){
        strcat(data_, string2.data_);
    }
    else{
        char *result = new char[strlen(data_) + strlen(string2.data_) + 1];
        strcpy(result, data_);
        strcat(result, string2.data_);
        delete[] data_;
        data_ = result;
    }
    size_ = strlen(data_);
    capacity_ = size_;
}

SimpleString SimpleString::substr(size_t pos, size_t count) const{
    char *text = new char[size_+1];
    strcpy(text ,&data_[pos]);
    if(count != 0) text[count] = '\0';
    SimpleString string = SimpleString(text);
    delete[] text;
    return string;
}
int SimpleString::compare(const SimpleString& string1, bool case_sensitive) const {
    if(case_sensitive) {
        if (!strcmp(data_, string1.data_)) return 0;
        if(strcmp(data_, string1.data_) < 0) return -1;
        else return 1;
    }
    else{
        char *s1 = data_;
        for(;*s1;s1++) if(*s1 >= 'A' && *s1 <= 'Z') *s1 -= 'A'-'a';
        char *s2 = (char*) string1.data_;
        for(;*s2;s2++) if(*s2 >= 'A' && *s2 <= 'Z') *s2 -= 'A'-'a';
        if (!strcmp(data_, string1.data_)) return 0;
        if(strcmp(data_, string1.data_) < 0) return -1;
        else return 1;
    }
}

```




$ 1+2=5 $
$$ 2*4/3=0
1-3
$$

<YouTube title="Abc" linkOrId="m8VSYcLqaLQ" startSeconds="33" endSeconds="44" />


<Note title="Tytul artykulu">
tekst artykulu tekst artykulutekst artykulu
tekst artykulu
</Note>

$$ 2*4/3=0
1-3
$$


```Python
print("Hello world!")

public class Divide_Input
    {
        public static void find_end(List<string> lines, List<string> com, int i, string end)
        {
            for (; i < lines.Count; i++)
            {
                if (Regex.Match(lines[i], end).Success)
                {
                    com[2] = i.ToString();
                    break;
                }
            }
        }

        /// <summary>
        /// Identify component types within the file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<List<string>> divide(string path)
        {
            List<string> lines = File.ReadAllLines(path).ToList();

            var beginings1 = from line in lines.Select((Value, Index) => new { Value, Index })
                             where Regex.Match(line.Value, "^<(.*) ", RegexOptions.Singleline).Success
                             select new List<string> {line.Index.ToString(),
                                                    line.Value.Substring(line.Value.IndexOf("<")+1,
                                                    line.Value.IndexOf(" ")-1)};


            var beginings2 = from line in lines.Select((Value, Index) => new { Value, Index })
                             where Regex.Match(line.Value, "^```(.+)", RegexOptions.Singleline).Success
                             select new List<string> { line.Index.ToString(), line.Value.Split("\n")[0].Remove(0,3) };


            var beginings3 = from line in lines.Select((Value, Index) => new { Value, Index })
                             where Regex.Match(line.Value, @"^(([^\$]+\$ )|(\$ ))", RegexOptions.Singleline).Success
                             select new List<string> { line.Index.ToString(), "InlineLatex", line.Index.ToString() };


            var beginings4 = from line in lines.Select((Value, Index) => new { Value, Index })
                             where Regex.Match(line.Value, @"\$\$", RegexOptions.Singleline).Success
                             select new List<string> { line.Index.ToString(), "BlockLatex" };

            var beginings41 = beginings4.ToList();
            for (int j = 1; j < beginings4.Count(); j += 2) beginings41.RemoveAt(j);


            var components = beginings1.Concat(beginings2).Concat(beginings3).Concat(beginings41).ToList();

            foreach (var com in components)
            {
                com.Add("");
                int i = Int32.Parse(com[0]);
                string name = com[1];
                if (name == "BlockLatex") find_end(lines, com, i + 1, @"^\$\$(.*)");
                else if (name == "Header" || name == "YouTube") find_end(lines, com, i, @"/>");
                else if (name == "Note" || name == "Warning" || name == "Info" || name == "DeepDive")
                    find_end(lines, com, i, string.Format(@"</{0}>", name));
                else if(name != "InlineLatex") find_end(lines, com, i + 1, "^```");
            }

            SortedSet<int> lines_found = new SortedSet<int>(); 
            foreach(var com in components){
                for(int i = Int32.Parse(com[0]); i <= Int32.Parse(com[2]); i++){
                    lines_found.Add(i);
                }

            }
            for(int i=0; i < lines.Count(); i++) {
                // if(!lines_found.Contains(i) && lines[i] != ""){
                if(!lines_found.Contains(i)){
                    int beginning = i;
                    int end = i;
                    while(!lines_found.Contains(i) && i+1 < lines.Count()){
                        end = i;
                        i++;
                    }
                    components.Add(new List<string>{beginning.ToString(), "Markdown", end.ToString()});
                }
            }

            components.Sort((x, y) => Int32.Parse(x[0]).CompareTo(Int32.Parse(y[0])));
            return components;
        }

        /// <summary>
        /// Replace component types with their content
        /// </summary>
        /// <param name="path"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        /// 
        public static List<List<string>> return_components(string path, List<List<string>> info)
        {
            List<string> lines = File.ReadAllLines(path).ToList();
            List<List<string>> components = new List<List<string>>();

            for (int i = 0; i < info.Count; i++)
            {
                components.Add(new List<string>());
                string content = "";
                for (int j = Int32.Parse(info[i][0]); j <= Int32.Parse(info[i][2]); j++)
                {
                    content += lines[j];
                    content += "\n";
                }
                content = content.Remove(content.Length - 1);
                components[i].Add(info[i][0]);
                components[i].Add(content);
            }
            return components;
        }
        public static void get_feedback(string path, List<List<string>> info)
        {
            List<string> lines = File.ReadAllLines(path).ToList();

            for (int i = 0; i < info.Count; i++)
            {
                ComponentType component;
                if(Enum.IsDefined(typeof(ComponentType), info[i][1])){
                    Enum.TryParse(info[i][1], out component);
                }
                else component = ComponentType.Markdown;

                string content = "";
                for (int j = Int32.Parse(info[i][0]); j <= Int32.Parse(info[i][2]); j++)
                {
                    content += lines[j];
                    content += "\n";
                }
                content = content.Remove(content.Length - 1);

                string message = Linter.Lint(content, component);
                if(message != null){
                    Console.WriteLine("-------------------------------------------");
                    Console.WriteLine("Component type: " + info[i][1]);
                    Console.WriteLine("Beginning at line: " + info[i][0]);
                    Console.WriteLine("Message:");
                }

                
            }
        }
    }
```

<Note>
tekst artykulu
tekst artykulu
</Note>

<Warning title="Tytul artykulu">tekst artykulu tekst artykulutekst artykulu
tekst artykulu
tekst artykulu
tekst artykulu
blb

hh
</Warning>

<Info title="Tytul artykulu">
tekst artykulu tekst artykulutekst artykulu
tekst artykulu
tekst artykulu
<Warning title="Tytul artykulu">
tekst artykulu tekst artykulutekst artykulu
blb

hh
</Warning>


hh
</Info>

<DeepDive title="Tytul artykulu">tekst artykulu tekst artykulutekst artykulu
tekst artykulu
tekst artykulu
tekst $ 2=3 $
tekst artykulu
blb

```sql
-- Create Customers table
CREATE TABLE Customers (
    CustomerID INT PRIMARY KEY,
    FirstName VARCHAR(50),
    LastName VARCHAR(50),
    Email VARCHAR(100)
);

-- Create Orders table
CREATE TABLE Orders (
    OrderID INT PRIMARY KEY,
    CustomerID INT,
    OrderDate DATE,
    TotalAmount DECIMAL(10, 2),
    FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID)
);

-- Create Products table
CREATE TABLE Products (
    ProductID INT PRIMARY KEY,
    ProductName VARCHAR(100),
    Price DECIMAL(10, 2)
);

-- Insert sample data into Customers table
INSERT INTO Customers (CustomerID, FirstName, LastName, Email)
VALUES (1, 'John', 'Doe', 'john.doe@example.com'),
       (2, 'Jane', 'Smith', 'jane.smith@example.com');

-- Insert sample data into Orders table
INSERT INTO Orders (OrderID, CustomerID, OrderDate, TotalAmount)
VALUES (1, 1, '2024-04-28', 100.00),
       (2, 2, '2024-04-29', 150.00);
```

hh
</DeepDive>




<Info title="Tytul artykulu">
tekst artykulu tekst artykulutekst artykulu
tekst artykulu
tekst artykulu

<Warning title="Tytul artykulu">
tekst artykulu tekst artykulutekst artykulu
```c
void setup() 
{ 
    gameover = 0; 
  
    // Stores height and width 
    x = height / 2; 
    y = width / 2
label1: 
    fruitx = rand() % 20; 
    if (fruitx == 0) 
        goto label1; 
label2: 
    fruity = rand() % 20; 
    if (fruity == 0) 
        goto label2; 
    score = 0; 
} 
  
// Function to draw the boundaries 
void draw() 
{ 
    system("cls"); 
    for (i = 0; i < height; i++) { 
        for (j = 0; j < width; j++) { 
            if (i == 0 || i == width - 1 
                || j == 0 
                || j == height - 1) { 
                printf("#"); 
            } 
            else { 
                if (i == x && j == y) 
                    printf("0"); 
                else if (i == fruitx 
                         && j == fruity) 
                    printf("*"); 
                else
                    printf(" "); 
            } 
        } 
        printf("\n"); 
    } 
  
    // Print the score after the 
    // game ends 
    printf("score = %d", score); 
    printf("\n"); 
    printf("press X to quit the game")
} 
```

blb
hh
</Warning>

hh
</Info>

<>



<DeepDive title="Tytuł nowy">
gg
Polska etnicznym monolitem? Nie na Górnym Śląsku

Po upływie półtora roku Główny Urząd Statystyczny opublikował wyniki spisu powszechnego z 2021 r. w zakresie struktury narodowo-etnicznej i języka kontaktów domowych. Na pierwszy rzut oka Polska po raz kolejny jawi się jako etniczny monolit. 97,7% spisanych zadeklarowało narodowość polską. Na Górnym Śląsku nie jest to jednak takie proste.

Sięgnijmy najpierw do historii poprzednich cenzusów w III RP. Pierwszy taki spis odbył się, gdy prezydentem Polski był Aleksander Kwaśniewski, tj. w 2002 r. Narodowość śląską zadeklarowało wówczas 173 tys. osób. To, że coś jest na rzeczy, wiadomo było już od 1996 r., kiedy podjęto próbę rejestracji Związku Ludności Narodowości Śląskiej. Polskie sądy ostatecznie tego odmówiły, tłumacząc wnioskodawcom, że ich narodowość nie istnieje.

W kolejnym spisie z 2011 r. narodowych Ślązaków było już 847 tys. Trudno porównywać te wyniki, ponieważ GUS dopuścił składanie podwójnych deklaracji tożsamościowych. W 2011 r. 376 tys. uznało się wyłącznie za Ślązaków, ale większa część ankietowanych (471 tys.) w różnej kolejności łączyła identyfikacją śląską z inną, głównie polską. W tym samym spisie 529,4 tys. osób zadeklarowało, że używa w domu języka śląskiego.

Mniej Ślązaków? Ich liczba ciągle robi wrażenie

Na okazję nowego spisu w 2021 r. śląskonarodowym (ślązackim) aktywistom, na co dzień bardzo ze sobą skonfliktowanym, udało się przeprowadzić wspólną kampanię pod nazwą „Ślōnsko Sztama”. Jej celem było przypomnienie ludziom, że mają możliwość zadeklarowania narodowości śląskiej, oraz poinstruowanie, jak to zrobić. Nie było to takie łatwe, ponieważ w przeciwieństwie do innych mniejszości opcję „narodowość śląska” trzeba było samemu wpisać z klawiatury. Ot, niewinna szykana ze strony państwa wobec kilkuset tysięcy swoich obywateli.

Wyniki przyszły po półtora roku oczekiwania. Swoją identyfikację narodowo-etniczną jako śląską zadeklarowało łącznie 585,7 tys. osób. Z tej liczby 231,8 tys. podało ją na pierwszym miejscu, a 353,9 tys. – na drugim. Jako język kontaktów domowych śląski podało 457,9 tys. osób, z czego 53,3 tys. twierdzi, że używa w domu wyłącznie tego języka. Są to nadal liczby wstępne, mogą więc ulec pewnej korekcie.
Co więcej, nadal nie wiemy, ile osób zadeklarowało wyłącznie tożsamość śląską. W porównaniu do poprzedniego spisu łączna liczba osób deklarujących się jako Ślązacy spadła o 30,8%. Mniejszy spadek zaliczył język śląski – liczba jego zadeklarowanych użytkowników zmniejszyła się o 13,5%.

Trzeba wyraźnie podkreślić, czego nam te statystyki nie mówią. Nie dowiemy się z nich, ile jest osób pośród autochtonów i wrosłych w region Polaków, którzy czują się Ślązakami, ale nie deklarują odrębnej tożsamości narodowej lub etnicznej. Podobnie ciemną liczbą są ci, którzy godajōm po naszymu, ale uważają to po prostu za mówienie w dialekcie języka polskiego.

Co do tego, że jest to milcząca większość, nie ma wątpliwości opcja polskośląska, która właśnie przypomniała sobie o swoim istnieniu. Na co dzień nie przejawia prawie żadnej aktywności, ale teraz z tryumfem oznajmiła klęskę wrażych „ślązakowców”. „O takie wyniki spisu nic nie robiliśmy” – mogłoby brzmieć ich hasło.

Piotr Spyra, były wicewojewoda śląski i chyba najpoważniejsza osoba w tym środowisku, komentował tę sytuację następująco: „Tożsamość narodowa i etniczna jest zjawiskiem dynamicznym, a kolejne spisy powszechne pokazują kierunki tej dynamiki. Porównanie liczby wskazań na narodowość śląską w 2 ostatnich spisach […] pokazuje w sposób jednoznaczny, że zwolennicy przekształcenia nas, Ślązaków, w odrębną grupę etniczną lub narodową są w głębokiej defensywie”.

Opcja śląskonarodowa pociesza się, że, „ponad 0,5 mln”, choć nie brzmi może tak dobrze jak „prawie 1 mln”, jest nadal efektowną liczbą. Narodowych Ślązaków jest nie tylko kilkadziesiąt razy więcej niż wszystkich oficjalnych mniejszości etnicznych w Polsce liczonych łącznie, ale także więcej od niejednego narodu europejskiego.

Nie zmienia to jednak faktu, że spis wykazał znaczny spadek śląskich deklaracji. Na usta ciśnie się więc pytanie: Kaj sōm te wszyjske Ślōnzoki?

Ślązacy to gatunek na wymarciu?

Pierwsze, najprostsze wyjaśnienie głosi, że wyniki z 2011 r. były znacząco przeszacowane. Przypomnijmy bez wchodzenia w szczegóły, że tamten spis był robiony po kosztach i „powszechnym” był tylko z nazwy. Przy najnowszym cenzusie GUS dotarł podobno do 95% populacji, więc nowe wyniki powinny być bardziej miarodajne.

Są też tacy, którzy lepszy wynik w 2011 r. tłumaczą „efektem Kaczyńskiego”. Chodzi o niefortunną wypowiedź prezesa PiS-u, w której narodowych Ślązaków nazwał „zakamuflowaną opcją niemiecką”. Rozpętała się medialna burza i pojawiły się pomysły, aby zadeklarować się jako Ślązacy na złość „Kaczorowi”. Iloma osobami kierowała taka motywacja, tego nie wiemy. Na szczęście w 2021 r. takiego wzmożenia już nie było.
Odrębność kulturowa autochtonicznych Ślązaków polega na silnie rozwiniętym, niemalże materialnym poczuciu tożsamości, które wyraża się w więziach regionalnych, elementach kultury i szczególnie używaniu gwar śląskich. Tożsamość kulturowa Ślązaków została utrwalona w wyniku zmiennych losów historycznych, a także pogranicznego charakteru społeczności[18].

Przy analizowaniu sytuacji socjologicznej panującej na początku XX wieku podkreśla się, że Ślązacy ze Śląska Opolskiego nie posiadali rozwiniętej świadomości narodowej i przekonania o uczestnictwie we wspólnocie narodowej. Elementem jednoczącym tę zbiorowość była więź regionalna. Poza nieliczną grupą otwarcie manifestującą swą polskość, autochtoniczni Ślązacy ze Śląska Opolskiego pielęgnowali swe polskie elementy kulturowe nie jako wyraz łączności z narodem polskim, lecz jako elementy miejscowego folkloru i swojskiej kultury, dzięki której mogli odróżnić się od Niemców[18][19].

Z drugiej strony znane są wyniki spisów narodowych z początku XX wieku (1910) gdzie 53% ludności Górnego Śląska (rejencji opolskiej) zadeklarowało język polski jako język ojczysty[20].

Jednym z elementów tożsamości kulturowej rodzimych Ślązaków są używane przez nich gwary, które od wieków pozwalały rozpoznawać swoich i obcych. Wzmacniają one spójność grupy, a ich wyzbycie się równoznaczne jest z porzuceniem wspólnoty. Jednakże należy zwrócić uwagę, że autochtoni z ludnością napływową używają języka literackiego, a gwarami posługują się w rodzinie i obrębie własnej zbiorowości[21].

Wśród autochtonicznych Ślązaków istnieje silna więź z własnym małym terytorium i jego społecznością lokalną. Badania socjologiczne przed 1990 r. wykazały jednak, że ponad 50% autochtonów na Śląsku Opolskim za ziemię rodzinną uważa swoją miejscowość lub okolice. Wykazano, że Ślązaków cechują silna orientacja lokalna i regionalna i słaba orientacja na Polskę jako ojczyznę. Charakterystyczne są przywiązanie do rodzinnych stron i niechęć do migracji w inne regiony Polski. Podstawą emocjonalnej więzi do rodzinnych stron jest silne poczucie zakorzenienia swojej rodziny, która mieszka tu od pokoleń[22].

Współcześnie zauważalne są przyspieszone procesy unifikacji tożsamości kulturowej Ślązaków, związane przede wszystkim z wysokim współczynnikiem urbanizacji, upowszechnieniem się kultury masowej, migracjami oraz łatwym dostępem do informacji[23].
Ślązacy posługują się etnolektem śląskim, powiązanym z[e] językiem polskim literackim, językiem niemieckim i czeskim. Śląszczyzna od 2007 r. posiada kod ISO 639-3 „szl”[24]. Mieszkańcy Dolnego Śląska, do czasu wysiedlenia po II wojnie światowej, posługiwali się także dialektem śląskim języka niemieckiego do dziś używanego w Niemczech, Górnych Łużycach oraz przez mniejszość niemiecką w Polsce – posiada on kod ISO 639-3 „sli”
Spis z 1910 wykazał blisko 582 tys. osób podających język polski jako ojczysty i ponad 51 tys. polski i niemiecki (dwujęzyczni). Przeprowadzane spisy ludności podporządkowane były celom politycznym, spis z roku 1925 wykazał już tylko ponad 151 tys. osób podających język polski jako ojczysty i 384 tys. dwujęzycznych. Był to wynik polityki germanizacyjnej, presji politycznej, ekonomicznej i administracyjnej[30].

Na obszarze przyłączonym do Polski w 1945 r., tj. Śląsku Opolskim i Dolnym Śląsku, w chwili zakończenia działań wojennych wśród niemieckiej ludności przetrwało 891 117 osób określonych jako „polska ludność rodzima” – z czego na Śląsku Opolskim 827 500 oraz 63 617 w woj. wrocławskim (dużym)[31].

Według danych z 15 września 1946 r. na Śląsku Opolskim polska ludność rodzima obejmowała 850 tys. osób, a w (dużym) woj. wrocławskim 15 tys. osób zweryfikowanych[31].

Po 1945 r. na Śląsku mieszkało 850 tys. zweryfikowanych autochtonów z niemieckiej części Śląska oraz 1,2 mln autochtonów z polskiej części Górnego Śląska[32][33]. Znaczna część pozytywnie zweryfikowanych Ślązaków ze Śląska Opolskiego wyemigrowała po wojnie za granicę lub poczuwa się dziś do narodowości niemieckiej[32].

W 1991 r. podczas spisu powszechnego w Czechosłowacji narodowość śląską zadeklarowało 44 446 osób, głównie na Śląsku Opawskim, gdzie nie było możliwości zadeklarowania narodowości polskiej[
W czeskim spisie powszechnym w 2011 narodowość śląską zadeklarowało 12 231 osób, z tego najwięcej w kraju morawsko-śląskim – 11 317[35]. W 2001 r. w spisie powszechnym wśród obywateli Czech – 10 878 osób zadeklarowało narodowość śląską, a 51 968 obywateli narodowość polską[36]. Najwięcej obu rodzajów deklaracji było w kraju morawsko-śląskim, tj. 9753 osób deklarujących narodowość śląską oraz 38 908 osób deklarujących narodowość polską[37][38]. W powiecie Opawa, którego część obszaru należy historycznie do Śląska Opawskiego było 4486 osób deklarujących narodowość śląską i 297 osób deklarujących narodowość polską[39]. Natomiast w powiatach: Frýdek-Místek oraz Karviná, których część obszarów należy historycznie do Śląska Cieszyńskiego było łącznie 3580 osób deklarujących narodowość śląską oraz 37 117 osób deklarujących narodowość polską[40][41]. Choć we współczesnych Czechach to obywatel deklaruje swoją narodowość podczas spisu, oficjalne stanowisko rządu czeskiego mówi, iż Ślązacy nie spełniają wymogów potrzebnych do określenia ich jako odrębną mniejszość narodową, ale traktowani są jako mniejszość etniczna[42].

W 2002 r. podczas Narodowego Spisu Powszechnego, narodowość śląską w polskiej części Śląska zadeklarowały 173 153 osoby, w tym 148,5 tys. mieszkańców województwa śląskiego i 24,2 tys. osób w województwie opolskim[43]. W 2002 r. na obszarze polskiej części Śląska Cieszyńskiego (powiat cieszyński, powiat bielski, Bielsko-Biała) narodowość śląską zadeklarowało 1045 osób[7]. Zarzuty nierzetelności rachmistrzów przy Narodowym Spisie Powszechnym w 2002 r. podnosili: Ruch Autonomii Śląska (uważający dane ze spisu za zaniżone), przewodniczący Zjednoczenia Łemków oraz przedstawiciele Związku Ukraińców w Polsce[44][45].

W 2011 r. podczas Narodowego Spisu Powszechnego, narodowość śląską lub przynależność do śląskiej wspólnoty etnicznej w polskiej części Śląska zadeklarowało 809 000 osób, w tym 362 tys. osób zadeklarowało ją jako jedyną narodowość, 56 tys. jako pierwszą przy zadeklarowaniu również drugiej narodowości, 391 tys. jako drugą narodowość lub wspólnotę etniczną[46]. Najwięcej osób deklarujących przynależność do narodowości śląskiej lub do śląskiej grupy etnicznej mieszka w województwach śląskim i opolskim. W województwie śląskim w spisie powszechnym było 700 000 deklaracji śląskich, w tym 318 000 deklaracji wyłącznie śląskich oraz 382 000 deklaracji podwójnej identyfikacji (w tym 370 000 identyfikacji śląsko-polskich)[47]. W województwie opolskim deklaracji śląskich było łącznie 100 000, z czego 41 000 wyłącznie śląskich i 39 000 śląsko-polskich[48].

W 2021 r. podczas polskiego Narodowego Spisu Powszechnego śląską identyfikację narodowo-etniczną zadeklarowało 585 700 osób (spadek o 30,9% wobec spisu z 2011 roku), w tym 231,8 tys. osób jako identyfikację pierwszą (spadek o 47% wobec spisu z 2011 r.), a 353,9 tys. jako identyfikację drugą[49].
Podczas spisu powszechnego w Czechach w 2021 narodowość śląską zadeklarowało 31 301 osób[50], z tego 12 451 jako jedyną[51]. Na obszarze historycznego Śląska mieszkało 27 218 z nich (11 193 licząc wyłącznie deklaracje pojedyncze), co stanowiło 2,67% mieszkańców regionu (1,09% licząc wyłącznie deklaracje pojedyncze). Najwięcej zadeklarowanych Ślązaków mieszkało na ziemi hulczyńskiej (6291 osób stanowiących 9,70% populacji), gdzie znajdowały się też jedyne miejscowości, gdzie liczba śląskich deklaracji narodowościowych przekroczyła 15%: Darkovice (18,59%), Kobeřice (16,13%) i Hněvošice (16,02%). W liczbach bezwzględnych gminami o największej liczbie Ślązaków były miasta Ostrawa (3173 osób), Opawa (2989 osób) i Trzyniec (1154 osób)[52].

Na Słowacji w czasie spisu w 2021 roku narodowość śląską zadeklarowało 117 osób[53].

Ślązaków jako grupę etniczną wymienia The World Factbook[54].
</DeepDive>

<Note>
Uważaj na to!!!
</Note>

<Note>
I na to :
<Warning>
kkkaa sjnclsc ckmc
lldll
</Warning>
</Note>

<Note>
```py
def factorial_r(n):
    if n == 0:
        return 1
    else:
        return n*factorial_r(n-1)


def factorial_i(n):
    factorial = 1
    while n:
        factorial *= n
        n -= 1
    return factorial


def fibonacci_r(n):
    if n == 0:
        return 0
    if n == 1:
        return 1
    else:
        return fibonacci_r(n-1)+fibonacci_r(n-2)


def fibonacci_i(n):
    if n == 0:
        return n
    if n == 1:
        return n
    fib = 0
    a = 0
    b = 1
    while n > 1:
        fib = a+b
        support = b
        b = a+b
        a = support
        n -= 1
    return fib

```


</Note>

<DeepDive>
deep w DeepDive
kakk dmmmmmmmmmm dmdmdmm wkwkk
lllllls
wkms

W
</DeepDive>

$$\int_{a}^{b} \frac{1}{2} \left( \sum_{n=1}^{\infty} \frac{(-1)^{n+1}}{n^2} \right) e^{i \pi} \cdot \left( \begin{matrix} \alpha & \beta \\ \gamma & \delta \end{matrix} \right) \, dx = \sqrt{\frac{\pi}{2}} \cdot \left( \frac{\Gamma(\frac{1}{4})}{\Gamma(\frac{3}{4})} \right)$$

---
__Advertisement :)__

- __[pica](https://nodeca.github.io/pica/demo/)__ - high quality and fast image
  resize in browser.
- __[babelfish](https://github.com/nodeca/babelfish/)__ - developer friendly
  i18n with plurals support and easy syntax.

You will like those projects!

---

# h1 Heading 8-)
## h2 Heading
### h3 Heading
#### h4 Heading
##### h5 Heading
###### h6 Heading


## Horizontal Rules

___

---

***


## Typographic replacements

Enable typographer option to see result.

(c) (C) (r) (R) (tm) (TM) (p) (P) +-

test.. test... test..... test?..... test!....

!!!!!! ???? ,,  -- ---

"Smartypants, double quotes" and 'single quotes'


## Emphasis

**This is bold text**

__This is bold text__

*This is italic text*

_This is italic text_

~~Strikethrough~~


## Blockquotes


> Blockquotes can also be nested...
>> ...by using additional greater-than signs right next to each other...
> > > ...or with spaces between arrows.


## Lists

Unordered

+ Create a list by starting a line with `+`, `-`, or `*`
+ Sub-lists are made by indenting 2 spaces:
  - Marker character change forces new list start:
    * Ac tristique libero volutpat at
    + Facilisis in pretium nisl aliquet
    - Nulla volutpat aliquam velit
+ Very easy!

Ordered

1. Lorem ipsum dolor sit amet
2. Consectetur adipiscing elit
3. Integer molestie lorem at massa


1. You can use sequential numbers...
1. ...or keep all the numbers as `1.`

Start numbering with offset:

57. foo
1. bar


## Code

Inline `code`

Indented code

    // Some comments
    line 1 of code
    line 2 of code
    line 3 of code


Block code "fences"

```
Sample text here...
```

Syntax highlighting

``` js
var foo = function (bar) {
  return bar++;
};

console.log(foo(5));
```

## Tables

| Option | Description |
| ------ | ----------- |
| data   | path to data files to supply the data that will be passed into templates. |
| engine | engine to be used for processing templates. Handlebars is the default. |
| ext    | extension to be used for dest files. |

Right aligned columns

| Option | Description |
| ------:| -----------:|
| data   | path to data files to supply the data that will be passed into templates. |
| engine | engine to be used for processing templates. Handlebars is the default. |
| ext    | extension to be used for dest files. |


## Links

[link text](http://dev.nodeca.com)

[link with title](http://nodeca.github.io/pica/demo/ "title text!")

Autoconverted link https://github.com/nodeca/pica (enable linkify to see)


## Images

![Minion](https://octodex.github.com/images/minion.png)
![Stormtroopocat](https://octodex.github.com/images/stormtroopocat.jpg "The Stormtroopocat")

Like links, Images also have a footnote style syntax

![Alt text][id]

With a reference later in the document defining the URL location:

[id]: https://octodex.github.com/images/dojocat.jpg  "The Dojocat"


## Plugins

The killer feature of `markdown-it` is very effective support of
[syntax plugins](https://www.npmjs.org/browse/keyword/markdown-it-plugin).


### [Emojies](https://github.com/markdown-it/markdown-it-emoji)

> Classic markup: :wink: :cry: :laughing: :yum:
>
> Shortcuts (emoticons): :-) :-( 8-) ;)

see [how to change output](https://github.com/markdown-it/markdown-it-emoji#change-output) with twemoji.


### [Subscript](https://github.com/markdown-it/markdown-it-sub) / [Superscript](https://github.com/markdown-it/markdown-it-sup)

- 19^th^
- H~2~O


### [\<ins>](https://github.com/markdown-it/markdown-it-ins)

++Inserted text++


### [\<mark>](https://github.com/markdown-it/markdown-it-mark)

==Marked text==


### [Footnotes](https://github.com/markdown-it/markdown-it-footnote)

Footnote 1 link[^first].

Footnote 2 link[^second].

Inline footnote^[Text of inline footnote] definition.

Duplicated footnote reference[^second].

[^first]: Footnote **can have markup**

    and multiple paragraphs.

[^second]: Footnote text.


### [Definition lists](https://github.com/markdown-it/markdown-it-deflist)

Term 1

:   Definition 1
with lazy continuation.

Term 2 with *inline markup*

:   Definition 2

        { some code, part of Definition 2 }

    Third paragraph of definition 2.

_Compact style:_

Term 1
  ~ Definition 1

Term 2
  ~ Definition 2a
  ~ Definition 2b


### [Abbreviations](https://github.com/markdown-it/markdown-it-abbr)

This is HTML abbreviation example.

It converts "HTML", but keep intact partial entries like "xxxHTMLyyy" and so on.

*[HTML]: Hyper Text Markup Language

### [Custom containers](https://github.com/markdown-it/markdown-it-container)

::: warning
*here be dragons*
:::
