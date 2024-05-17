/***************************************************************************************************
* Name : Snake																														   *
* Data : 09-14-2010																				   *
* Wersja : 1.3																					   *
															   *
****************************************************************************************************/

#include "stdafx.h"						   				 // Używa Visual Studio
#include <iostream>										 // Używa cout/cin
#include <conio.h>										 // Używa funkcje getch()
#include <windows.h>					 				 // Używa funkcji windows
#include <ctime>						 				 // Używa funkcji czas (prędkość gry)
#include <vector>										 // Używa funkcji Pozycje
#include <fstream>									     // Używa funkcji do wysokiego wyniku
#include <string>



using namespace std;
#define WIDTH 25										// Długość Plańszy
#define HEIGHT 20										// Szerokość Plańszy
char Level[HEIGHT][WIDTH];


// Deklaracja funkcji
void Initialize(int rozmiar, int snakeX[], int snakeY[]);														// Inicjacja Węża
void ClearScreen();																							// Jasność obrazu
void BudowaPoziomu();																							// Budowa Poziomu
void RysunekPlańszy();																								// Rysunek gry
void Poruszanie(int &rozmiar, int snakeX[], int snakeY[], int tempX[], int tempY[], int &item_count, int &punkty); // Aktualizacja pętli gry
void PrędkośćPoruszania();																							// Aktualizacja Prędkości węża
void Pozycje(vector<int> &itemX, vector<int> &itemY, int &item_count, int snakeX[], int snakeY[], int &rozmiar, int tailX, int tailY, int &punkty);	// Powiększanie i odżywianie się Węża
bool IsGameOver(int snakeX[], int snakeY[], int rozmiar);														// Funkcja Koniec Gry																	
void bialy();																								// Kolor nieaktywnych opcji	
void magnetowy();																							// Kolor aktywnej opcji																					
void PodMenu();																								// Wybór Poziomu Trudności																						
void Menu();																								// Menu główne
void snake();																								// Rozpoczęcie Gry
void Opcje();																								// Opcje
void Aktywne();																								// Zmiana koloru opcji Aktywnych
void Nieaktywne();																							// Zmiana koloru opcji Nieaktywnych
void GameOver();																							// Funkcja Koniec Gry
void Gwiazdka();																							// Gwiazdka po lewej stronie
void Gwiazdka2();																							// Gwiazdka po prawej stronie
void Sterowanie();																							// Wyświetla informacje o sterowaniu


// ZMIENNE
int n = 1, z;											// Poruszanie się klawiaturą w PodMenu
int m = 1;												// Poruszanie się klawiaturą w Menu
int o = 1;												// Poruszanie się klawiaturą w Opcjach
int p = 1;												// Poruszanie się klawiaturą w Zmianie koloru opcji Aktywnych
int r = 1;												// Poruszanie się klawiaturą w Zmianie koloru opcji Nieaktywnych
int x = 1;												// Poruszanie się klawiaturą w Funkcji GameOver
int nieaktywny = 1;										// Zmienna pozwalające zmiane Koloru wyświetlania opcji Nieaktywnych
int aktywny = 1;										// Zmienna pozwalające zmiane Koloru wyświetlania opcji Aktywnych

																								


// PODMENU
void PodMenu()
{
	
	
	while(1)
	{
		system("cls");									// Czyszczenie obrazu
		bialy();
		printf("\n\n\n\t\t\tWYBIERZ POZIOM TRUDNOSCI :      \n\n\n");		
		
		if(n==1){magnetowy();Gwiazdka();printf(" Latwy ");Gwiazdka2();}
		if(n==1){bialy(); printf("\t\t\t\t  Sredni\n");}
		if(n==1){bialy(); printf("\t\t\t\t  Trudny\n");}
		if(n==2){bialy(); printf("\t\t\t\t  Latwy\n");}
		if(n==2){magnetowy();Gwiazdka();printf(" Sredni ");Gwiazdka2();}		
		if(n==2){bialy(); printf("\t\t\t\t  Trudny\n");}		
		if(n==3){bialy(); printf("\t\t\t\t  Latwy \n");}
		if(n==3){bialy(); printf("\t\t\t\t  Sredni \n");}
		if(n==3){magnetowy();Gwiazdka();printf(" Trudny ");Gwiazdka2();}

		z = _getch();

		if(z == 72){n--;}
		if(z == 80){n++;}
		if(n == 4)if(n>3){n--;}
		if(n == 0){n =+ 3;}
		if((z == 13) && (n == 3))  Menu();
		if((z == 13) && (n == 2))  Menu();
		if((z == 13) && (n == 1))  Menu();
	}
}


// Menu główne
void Menu()
{
	while(1)
	{
		bialy();
		system("cls");
		printf("\t\t  SSSS                       kk\n");
		printf("\t\t SS   SS                     kk\n");
		printf("\t\t SS   SS                     kk\n");
		printf("\t\t SS        nnnnn     aaaa    kk  kk    eeee\n");
		printf("\t\t  SSS      nn  nn   aa  aa   kk kk    ee  ee\n");
		printf("\t\t    SSS    nn  nn     aaaa   kkkk     eeeeee\n");
		printf("\t\t      SS   nn  nn    aa aa   kkk      ee\n");
		printf("\t\t SS   SS   nn  nn   aa  aa   kkkk     ee\n");
		printf("\t\t SS   SS   nn  nn   aa  aa   kk kk    ee  ee\n");
		printf("\t\t  SSSSS    nn  nn    aaaaa   kk  kk    eeee\n\n\n");	
		
		
		if(m==1){magnetowy();}printf("\t\t\t\t ROZPOCZNIJ GRE \n");bialy();
	    if(m==2){magnetowy();}printf("\t\t\t\t MENU OPCJI \n");bialy();
	    if(m==3){magnetowy();}printf("\t\t\t\t WYJSCIE \n");bialy();
		z = _getch();

		if(z == 72){m--;}
		if(m == 0){m =+ 3;}
		if(z == 80){if(m<3){m++;}}						// Ograniczenie z dołu
		if((z == 13) && (m == 3)) exit(1);				// Wyjście z programu
		if((z == 13) && (m == 2)) Opcje();				// Menu opcji
		if((z == 13) && (m == 1)) Sterowanie();			// Rozpoczęcie Gry	
	}
}

// Informacja
void Sterowanie()
{
	system("cls");
	cout << "\n\n\t\t\t STEROWANIE 'a','w','s','d'\n\t\t\t\t Pauza - 'p'\n\n";
	cout << "\t\tAby kontunuowac nacisnij dowolny klawisz ...\n\n\t\t\t\tMlej gry :-)";
	_getch();											// Oczekiwanie na klawisz
	snake();
}

// MENU
int main()
{	
	PodMenu();											// Przekierowanie do PodMenu
}

// Snake
void snake()
{
	int rozmiar = 3;										// Rozmiar początkowy Węża to 3
	int item_count = 0;
	int punkty = 0;
	int snakeX[100], snakeY[100], tempX[100], tempY[100];
	

	system("cls");										// Czyszczenie Ekranu
	Initialize(rozmiar, snakeX, snakeY);
	BudowaPoziomu();
	Poruszanie(rozmiar, snakeX, snakeY, tempX, tempY, item_count, punkty);	

	_getch();	
}

// Opcje
void Opcje()
{
	while(1)
	{
		system("cls");									// Czyszczenie Ekranu
		bialy();		
		printf("\n\n\n\t\t\t\t OPCJE :      \n\n\n");		
		if(o == 1){magnetowy();}printf("\t\t\t Kolor Nieaktywnych opcji \n");bialy();
		if(o == 2){magnetowy();}printf("\t\t\t Kolor Aktywnych opcji \n");bialy();
		if(o == 3){magnetowy();}printf("\t\t\t Poziom Trudnosci \n");bialy();
		if(o == 4){magnetowy();}printf("\t\t\t Powrot\n");bialy();		
		z = _getch();

		if(z == 72){o--;}
		if(o == 0){o =+ 4;}
		if(z == 80){if(o<4){o++;}}						// Ograniczenie z dołu
		if((z == 13) && (o == 3))  PodMenu();
		if((z == 13) && (o == 2))  Aktywne();
		if((z == 13) && (o == 1))  Nieaktywne();
		if((z == 13) && (o == 4)) Menu();
	}
}

// Aktywne opcje
void Aktywne()
{
	while(1)
	{
		system("cls");									// Czyszczenie Ekranu
		bialy();
		cout << endl << "\n\n\t\t\tWYBIERZ KOLOR OPCJI AKTYWNYCH :      \n\n\n";
		if(p == 1){magnetowy();}printf("\t\t\t\t Orginalny \n");bialy();
		if(p == 2){magnetowy();}printf("\t\t\t\t Bialy \n");bialy();
		if(p == 3){magnetowy();}printf("\t\t\t\t Oliwkowy \n");bialy();
		if(p == 4){magnetowy();}printf("\t\t\t\t Niebieski \n");bialy();
		if(p == 5){magnetowy();}printf("\t\t\t\t Zielony \n");bialy();
		if(p == 6){magnetowy();}printf("\t\t\t\t Zolty \n");bialy();
		if(p == 7){magnetowy();}printf("\t\t\t\t Fioletowy \n");bialy();
		if(p == 8){magnetowy();}printf("\n\t\t\t\t Powrot\n");bialy();
		z = _getch();

		if(z == 72){p--;}
		if(p == 0){p =+ 8;}
		else
			if(z == 80){if(p<8){p++;}}				// Ograniczenie z dołu

			if((z == 13) && (p==1))					// Zmiana Koloru na Czerwony
		{
		    aktywny = 1;
			magnetowy();
		}
			if((z == 13) && (p == 2))				// Zmiana Koloru na Biały
			{
				aktywny = 2;
				magnetowy();
			}
		if((z == 13) && (p==3))						// Zmiana Koloru na Oliwkowy
		{
			aktywny = 3;
			magnetowy();
		}
		if((z == 13) && (p == 4))					// Zmiana Koloru na Niebieski
		{
			aktywny = 4;
			magnetowy();
		}
		if((z == 13) && (p == 5))					// Zmiana Koloru na Zielony
		{
			aktywny = 5;
			magnetowy();
		}
		if((z == 13) && (p == 6))					// Zmiana Koloru na Żółty
		{
			aktywny = 6;
			magnetowy();
		}
		if((z == 13) && (p == 7))					// Zmiana Koloru na Fioletowy
		{
			aktywny = 7;
			magnetowy();
		}		
		if((z == 13) && (p==8))						// Powrót do Menu
		{
			Opcje();
		}
			
					
	}
}

// Nieaktywne Opcje
void Nieaktywne()
{
	while(1)
	{
		system("cls");								// Czyszczenie Ekranu
		bialy();
		cout << endl << "\n\n\t\t\tWYBIERZ KOLOR OPCJI NIEAKTYWNYCH :      \n\n\n";
		if(r == 1){magnetowy();}printf("\t\t\t\t Orginalny \n");bialy();
		if(r == 2){magnetowy();}printf("\t\t\t\t Oliwkowy \n");bialy();
		if(r == 3){magnetowy();}printf("\t\t\t\t Czerwony\n");bialy();
		if(r == 4){magnetowy();}printf("\t\t\t\t Niebieski \n");bialy();
		if(r == 5){magnetowy();}printf("\t\t\t\t Zielony \n");bialy();
		if(r == 6){magnetowy();}printf("\t\t\t\t Zolty \n");bialy();
		if(r == 7){magnetowy();}printf("\t\t\t\t Fioletowy \n");bialy();
		if(r == 8){magnetowy();}printf("\n\t\t\t\t Powrot \n");bialy();
		z = _getch();

		if(z == 72){r--;}
		if(r == 0){r =+ 8;}
		if(z == 80){if(r<8){r++;}}					// Ograniczenie z dołu

		if((z == 13) && (r==1))						// Zmiana Koloru na Biały
		{
			nieaktywny = 1;
			bialy();
		}
		if((z == 13) && (r == 2))					// Zmiana Koloru na Oliwkowy
		{
			nieaktywny = 2;
			bialy();
		}
		if((z == 13) && (r==3))						// Zmiana Koloru na Czerwony
		{
			nieaktywny = 3;
			bialy();
		}
		if((z == 13) && (r == 4))					// Zmiana Koloru na Niebieski
		{
			nieaktywny = 4;
			bialy();
		}
		if((z == 13) && (r == 5))					// Zmiana Koloru na Zielony
		{
			nieaktywny = 5;
			bialy();
		}
		if((z == 13) && (r == 6))					// Zmiana Koloru na Żółty
		{
			nieaktywny = 6;
			bialy();
		}
		if((z == 13) && (r == 7))					// Zmiana Koloru na Fioletowy
		{
			nieaktywny = 7;
			bialy();
		}		
		if((z == 13) && (r==8))						// Powrót do Menu
		{
			Opcje();
		}
					
	}
}

// Kolory Aktywnych opcji
void magnetowy()
{	
	if(aktywny == 1)
	{		
		SetConsoleTextAttribute(GetStdHandle(STD_OUTPUT_HANDLE), FOREGROUND_RED );											// Czerwony
	}
	if(aktywny == 2)
	{
		SetConsoleTextAttribute(GetStdHandle(STD_OUTPUT_HANDLE), FOREGROUND_GREEN | FOREGROUND_RED | FOREGROUND_BLUE );		// Biały
	}
	if(aktywny == 3)
	{
		SetConsoleTextAttribute(GetStdHandle(STD_OUTPUT_HANDLE), FOREGROUND_GREEN | FOREGROUND_RED );						// Oliwkowy
	}
	if(aktywny == 4)
	{
		SetConsoleTextAttribute(GetStdHandle(STD_OUTPUT_HANDLE), FOREGROUND_BLUE );											// Niebieski
	}
	if(aktywny == 5)
	{
		SetConsoleTextAttribute(GetStdHandle(STD_OUTPUT_HANDLE), FOREGROUND_GREEN );										// Zielony
	}
	if(aktywny == 6)
	{
		SetConsoleTextAttribute(GetStdHandle(STD_OUTPUT_HANDLE), FOREGROUND_INTENSITY |FOREGROUND_GREEN | FOREGROUND_RED );	// Żółty	
	}
	if(aktywny == 7)
	{
		SetConsoleTextAttribute(GetStdHandle(STD_OUTPUT_HANDLE), FOREGROUND_BLUE | FOREGROUND_RED );						// Fioletowy
	}	
}

// Kolory Nieaktywnych opcji
void bialy()
{	
	if(nieaktywny == 1)
	{
		SetConsoleTextAttribute(GetStdHandle(STD_OUTPUT_HANDLE), FOREGROUND_GREEN | FOREGROUND_RED | FOREGROUND_BLUE );		// Biały		
	}
	if(nieaktywny == 2)
	{
		SetConsoleTextAttribute(GetStdHandle(STD_OUTPUT_HANDLE), FOREGROUND_GREEN | FOREGROUND_RED );						// Oliwkowy
	}
	if(nieaktywny == 3)
	{
		SetConsoleTextAttribute(GetStdHandle(STD_OUTPUT_HANDLE), FOREGROUND_RED );											// Czerwony	
	}
	if(nieaktywny == 4)
	{
		SetConsoleTextAttribute(GetStdHandle(STD_OUTPUT_HANDLE), FOREGROUND_BLUE );											// Niebieski
	}
	if(nieaktywny == 5)
	{
		SetConsoleTextAttribute(GetStdHandle(STD_OUTPUT_HANDLE), FOREGROUND_GREEN );										// Zielony
	}
	if(nieaktywny == 6)
	{
		SetConsoleTextAttribute(GetStdHandle(STD_OUTPUT_HANDLE), FOREGROUND_INTENSITY |FOREGROUND_GREEN | FOREGROUND_RED );	// Żółty
	}
	if(nieaktywny == 7)
	{
		SetConsoleTextAttribute(GetStdHandle(STD_OUTPUT_HANDLE), FOREGROUND_BLUE | FOREGROUND_RED );						// Fioletowy
	}
}

// Inicjalizacja Węża
void Initialize(int rozmiar, int snakeX[], int snakeY[])
{
	snakeX[0] = WIDTH/2;	snakeY[0] = 3;				// Wymiary Węża
	snakeX[1] = WIDTH/2;	snakeY[1] = 2;
	snakeX[2] = WIDTH/2;	snakeY[2] = 1;
	for(int i=3; i< 100; i++)
	{
		snakeX[i] = NULL;
		snakeY[i] = NULL;
	}

	// Pozycja węża 
	for(int i=0; i<rozmiar; i++)
		Level[snakeY[i]][snakeX[i]] = 'o' ;
}

// Budowa Poziomu
void BudowaPoziomu()
{
	for(int i=0; i<HEIGHT; i++)
	{
		for(int j=0; j<WIDTH; j++)
		{
			Level[0][j] = '*';
			Level[i][0] = '*';
			Level[i][WIDTH-1] = '*';
			Level[HEIGHT-1][j] = '*';
		}
	}
}
	

// Wyświetlenie Poziomu
void RysunekPlańszy()
{
	for(int i=0; i<HEIGHT; i++)
	{
		cout << endl;
		for(int j=0; j<WIDTH; j++)
			cout  << " " << Level[i][j];
			
	}
}

// Jasność Wyświetlania
void ClearScreen()
{
	HANDLE hOut;
	COORD Position;
	hOut = GetStdHandle(STD_OUTPUT_HANDLE);
	Position.X = 0;
	Position.Y = 0;
	SetConsoleCursorPosition(hOut, Position);
}

// Aktualizacja
void Poruszanie(int &rozmiar, int snakeX[], int snakeY[], int tempX[], int tempY[], int &item_count, int &punkty)
{
	int count = 0;
	char input = ' ';
	char previnput = 's';
	int tailX, tailY;
	bool gameOver = false;
	vector<int> itemX, itemY;

	while(!gameOver)									// Pętla aż gra się skończy
	{
		PrędkośćPoruszania();									// Prędkość

		// Dodanie ogona podczas poruszania
		tailY = snakeY[rozmiar-1];
		tailX = snakeX[rozmiar-1];

		// Usunięcie poprzednich śladów po ogonie
		Level[tailY][tailX] = ' ';
		Pozycje(itemX, itemY, item_count, snakeX, snakeY, rozmiar, tailX, tailY, punkty);

		// Kopia węża (z wyjątkiem ogona) do tablicy temp
		// Zostawienie pierwszego elementy
		for(int i=0; i<rozmiar-1; i++)
		{
			tempX[i+1] = snakeX[i];						// Kopiowanie wartości y
			tempY[i+1] = snakeY[i];						// Kopiowanie wartości x
		}

		// Kopia głowy do pierwszego elementy aby mógł zostać przeniesiony przez użytkownika
		tempX[0] = snakeX[0];
		tempY[0] = snakeY[0];

		// Kopiowanie tablicy temp
		// Pierwsze 2 elementy będą powielać głowy z samej pozycji X i Y
		for(int i=0; i<rozmiar; i++)
		{
			snakeX[i] = tempX[i];						// Kopiowanie wartości Y
			snakeY[i] = tempY[i];						// Kopiowanie wartości X
		}

		// Naciśnięcie klawiszy
		if(_kbhit())
			input = _getch();

		// WHILE MOVING DOWN
		if(previnput == 's')
		{
			snakeY[0] += 1;								// Zestaw głów na nowej pozycji
			if(input == 'a'	  || input == 'A')			// Idź w lewo
				previnput = 'a';
			else if(input == 'd'	|| input == 'D')	// Idź w prawo
				previnput = 'd';
		}
		// WHILE MOVING UP
		else if(previnput == 'w')
		{
			snakeY[0] -= 1;								// Zestaw głów na nowej pozycji
			if(input == 'a'		|| input == 'A')		// Idź w lewo
				previnput = 'a';
			else if(input == 'd'	|| input == 'D')	// Idź w prawo
				previnput = 'd';
		}
		// WHILE MOVING RIGHT
		else if(previnput == 'd')
		{
			snakeX[0] += 1;								// Zestaw głów na nowej pozycji
			if(input == 'w'		|| input == 'W')		// Idź w górę
				previnput = 'w';
			else if(input == 's'	|| input == 'S')	// Idź w dół
				previnput = 's';
		}
		// WHILE MOVING LEFT
		else if(previnput == 'a')
		{
			snakeX[0] -= 1;
			if(input == 'w'	 ||	input == 'W')			// Idź w góre
				previnput = 'w';
			else if(input == 's'	|| input == 'S')	// Idź w dół
				previnput = 's';
		}

		// PAUSE
		if(input == 'p' || input == 'P')
			_getch();

		// Zmiana pozycji Węża		
		for(int i=0; i<rozmiar; i++)			
			Level[snakeY[i]][snakeX[i]] = 'o';
			

		// Sprawdzenie czy gra się kończy
		gameOver = IsGameOver(tempX, tempY, rozmiar);
		if(gameOver == true)
		{			
			cout << "\a\a";
			break;
		}

		// Rysunek Plańszy
		ClearScreen();
		RysunekPlańszy();
	}
}

// Pozycje
void Pozycje(vector<int> &itemX, vector<int> &itemY, int &item_count, int snakeX[], int snakeY[], int &rozmiar, int tailX, int tailY, int &punkty)
{
	clock_t interval = clock() % 3000;	
	cout << "\t\t	Punkty: " << punkty << endl;
	
	// Pokaż przedmioty w odstępach czasu
	
	if(interval > 2800) 
	{			
		item_count++;
		srand((unsigned)time(NULL));
		// Pozycje znajdują się w przypadkowych miejscach
		itemX.push_back( rand() % (WIDTH-2)+1);
		itemY.push_back( rand() % (HEIGHT-2)+1);		
		Level[itemY.back()][itemX.back()] = '@';		
	}
	
	// Sprawdź pozycje
	for(int i=0; i<item_count; i++)
	{
		if( (snakeX[0] == itemX.at(i)) && (snakeY[0] == itemY.at(i)) )
		{
			punkty += 256;
			// Usuwanie pozycji
			item_count--;
			itemY.erase(itemY.begin()+1);
			itemX.erase(itemX.begin()+1);
			// Wąż rośnie
			rozmiar++;
			snakeX[rozmiar-1] = tailX;
			snakeY[rozmiar-1] = tailY;
			cout << "\a";
		}
	}
}

// Prędkość
void PrędkośćPoruszania()
{	
	if(n == 1)										// Poziom 1
	{
		Sleep(70);
		cout << endl << " Poziom Latwy";
	}
	else if(n == 2)									// Poziom 2
	{
		Sleep(50);
		cout << endl << " Poziom Sredni";
	}
	else if(n == 3)									// Poziom 3
	{
		Sleep(25);
		cout << endl << " Poziom Trudny";
	}
	
}

// Koniec gry
bool IsGameOver(int snakeX[], int snakeY[], int rozmiar)
{
	// Jeżeli zderzy się z ogonem węża
	for(int i=2; i<rozmiar; i++)
	{
		if( (snakeX[0] == snakeX[i]) && (snakeY[0] == snakeY[i]) )
		{
			cout << endl << "\t\t	Przegrales!";
			_getch();
			Sleep(2000);
			GameOver();
			return true;
		}
	}
	if((snakeX[0] == WIDTH-1)	||	(snakeY[0] == HEIGHT-1)	||	(snakeX[0] == 0)	||	(snakeY[0] == 0))
	{
		cout << endl << "\t\t	Przegrales!";
		Sleep(2000);
		_getch();
		GameOver();
		return true;
	}
	else
		return false;
}

// Funkcja GameOver
void GameOver()
{
	while(1)
	{
		system("cls");
		bialy();   
		printf("\n\n\n\n\t\t\t      Koniec Gry\n\n\n\t\t\t   GRASZ JESZCZE ?\n");		
		if(x==1){magnetowy();Gwiazdka();printf(" Tak ");Gwiazdka2();}
		if(x==1){bialy();printf("\t\t\t\t  Nie\n");}
		if(x==2){bialy(); printf("\t\t\t\t  Tak\n");}		
		if(x==2){magnetowy();Gwiazdka();printf(" Nie ");Gwiazdka2();}
		z = _getch();

		if(z == 72){if(x>1){x--;}}		
		if(z == 80){if(x<2){x++;}}
		if((z == 13) && (x == 2))  exit(1);
		if((z == 13) && (x == 1))  PodMenu();
	}
}

// Gwiazdka po Lewej
void Gwiazdka()
{
	cout << "\t\t\t\t*";
}

// Gwiazdka po Prawej
void Gwiazdka2()
{
	cout << "*\n";
}