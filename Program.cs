using System.Data;
namespace SomeTestProject  // Пространство имён нашего проекта

{  // Внутри пространства имён мы можем создавать некие классы

    class Program 
    {
        const int FIELD_WIDTH = 40; // Константа - ширина поля (40 символов)
        const int FIELD_HEIGHT = 15;  // Константа - высота поля (15 символов)
        const int MINE_COUNT = 40;   // Константа - количество мин (40 мин)

        enum CellType         // Объявление перечисления. Тип клетки.
        {
          EMPTY,   // Пустая (непосещённая) клетка
          VISITED,  // Посещённая клетка
          MINE, // Клетка с миной
          DISARMED,  // Клетка с обезвреженной миной
          
        }

        static CellType [,] field;

        static int px,py; // Координаты игрока
        static int tx, ty; //  Координаты сокровища

        static  void CreateField()
        {

           // Random
           var rand = new Random();  // Генератор псевдослучайных чисел. Будем использовать его для генерации координат клетки
            // Объявляем переменную которая будет иметь тип Random и создаём экземпляр класса Random

          field = new CellType [FIELD_WIDTH,FIELD_HEIGHT]; //  Создаётся массив с помощью оператора new. Указываем тип данных (CellType)
          //  и указываем размерность этого массива  [FIELD_WIDTH,FIELD_HEIGHT] ширина поля и высота поля.
          
           

          // Заполняем поле значениями EMPTY перечисления CellType прежде чем мы разместим мины
          for (int y = 0; y < FIELD_WIDTH; ++y)
            for (int x = 0; x < FIELD_HEIGHT; ++x)
              field[x,y] = CellType.EMPTY;

              // Размещаем на поле мины (цикл должен выполнть столько итераций сколько
              //  нужно разместить мин)
              for (int i = 0; i < MINE_COUNT; ++i) {       // Кол-во итераций = Кол-ву мин
                int mx,my;
                do {
                          // Определяем координаты размещения мин
                    mx = rand.Next() % FIELD_WIDTH;  // Присваиваем mx случайное число
                    my = rand.Next() % FIELD_HEIGHT;  // Присваиваем mу случайное число
              } while (field[mx,my] != CellType.EMPTY);  // Пока на поле в указанных координатах находится что-то отличное от  EMPTY. 
              // Т.е. если мина есть - идём в начало цикла  и выбираем другие координаты.


              field[mx,my] = CellType.MINE;  // Если ячейка является EMPTY то записываем в эту ячейку мину.
              }


              do {
                          // Определяем координаты размещения сокровища
                    tx = rand.Next() % FIELD_WIDTH;  // Присваиваем tx случайное число
                    ty = rand.Next() % FIELD_HEIGHT;  // Присваиваем tу случайное число
              } while (field[tx,ty] != CellType.EMPTY);

              do {
                          // Определяем координаты размещения игрока
                    px = rand.Next() % FIELD_WIDTH;  // Присваиваем px случайное число
                    py = rand.Next() % FIELD_HEIGHT;  // Присваиваем pу случайное число
              } while (field[px,py] != CellType.EMPTY && px != tx && py != ty); //Здесь важно не только отсутствие мины, но ещё чтобы 
              //  клетки с кладом и игроком не совпадали

        }

        static void ShowField()
        {
            Console.Clear(); // Очистка консоли
        //  Рисуем поле

            for (int row = 0; row < FIELD_HEIGHT; ++row) {
              for (int col = 0; col < FIELD_WIDTH; ++col) {
                  switch (field[col,row]) {   // Если значение на поле в координатах [col,row]
                    case CellType.EMPTY:   // EMPTY
                    case CellType.MINE:   //  либо  MINE
                      Console.Write("\u2592");  // Знакоместо заполненное точками
                      break;
                      case CellType.VISITED:  //  В случае если клетка посещена 
                        Console.Write("\u2591");  //  Знакоместо запоненное точками (но в меньшем количестве чем выше для EMPTY)
                        break;
                      case CellType.DISARMED:
                        Console.Write("\u2573");
                        break;
                  }
              }
              Console.WriteLine();  // Переход на новую строку консоли
            }
            (int cx, int cy) = Console.GetCursorPosition();
            Console.SetCursorPosition(px,py);  //  Вывод персонажа (кординаты)
            Console.Write("@");  //  Символ обозначающий персонажа
            Console.SetCursorPosition(cx,cy);
            Console.WriteLine("Расстояние до клада = {0}", Math.Sqrt((tx-px)*(tx-px)+(ty-py)*(ty-py))); // вычисляем расстояние до клада

            int mines = 0;  // Переменная которая будет хранить количество мин которое мы насчитали
            for (int dx = -1; dx <= 1; ++dx)  // dx смещение относительно текущей клетки
              for (int dy = -1; dy <= 1; ++dy){ // dy смещение относительно текущей клетки
                  int cx1 = px + dx;
                  int cy1 = py + dy;
                  if (cx1 < 0 || cy1 <0 || cx1 >= FIELD_WIDTH || cy1 >= FIELD_HEIGHT) // Если точка находится вне нашего поля
                      continue;
                  if (field[cx1, cy1] == CellType.MINE)  //  Если мина необезврежена - считаем её
                      mines++;
              }
              Console.WriteLine("Мин в соседних клетках - {0}", mines);

        }
  
        static void Main(String[]args)

        { 

          CreateField();
          ShowField();
           
        }
    }

}

/*
ИГРА
Имеется поле определённого размера. Размеры поля заданы константами.
На поле в определённых координатах зарыт клад.
По полю перемещается персонаж.
1) Где клад - неизвестно.
2) Известно расстояние до клада.
3) На поле разбросаны мины. Количество мин - константа
4) Где именно мины неизвестно
5) Известно количество мин вокруг текущей клетки.
Цель игры - найти клад не нарвавшись на мину.
Вывод - псевдографикой. Управление - от курсорных клавиш.
*/