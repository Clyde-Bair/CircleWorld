namespace CircleWorld {
    internal class Program {
        static void Main(string[] args) {
            while (true) {
                Map map = new Map();
                string[] loadedMap;
                byte currentMap = 0;
                int[] pos = {0,0,0};
                bool startup = true;
                bool noclip = false;
                loadedMap = map.LoadMap(currentMap);
                while (true) {
                    Console.CursorVisible = false;
                    Screen screen = new Screen();
                    screen.Generate();
                    if (!startup) {
                        RenderMap(loadedMap, pos, screen, noclip);
                        Console.ForegroundColor = ConsoleColor.White;
                        for (int i = 0; i < 4; i++) {
                            switch (i) {
                                case 0: Console.BackgroundColor = ConsoleColor.White; break;
                                case 1: Console.BackgroundColor = ConsoleColor.Gray; break;
                                case 2: Console.BackgroundColor = ConsoleColor.DarkGray; break;
                                case 3: Console.BackgroundColor = ConsoleColor.Black; break;
                            }
                            screen.Refresh();
                            Thread.Sleep(1000);
                        }
                        Thread.Sleep(1000);
                        Interface textbuh = new Interface();
                        textbuh.text = new string[] { "\"Im a circlular girl in the circlular world\"", "WELCOME TO CIRCLE WORLD!", "", "Use WASD to navigate the map!" };
                        textbuh.TextBox(screen, 0, 0);
                        startup = true;
                    }
                    if (currentMap != map.previousMap) {
                        loadedMap = map.LoadMap(currentMap);
                    }
                    Console.SetCursorPosition(0, 0);
                    Console.WriteLine($"CIRCLE WORLD - Map: {currentMap} - X_Pos: {-(pos[0] - screen.width)} - Y_Pos: {-(pos[1] - screen.height)} - Dir: {pos[2]}   ");
                    bool illegalInput = true;
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    switch (key.Key) {
                        case ConsoleKey.W: pos[1]++; pos[2] = 2; illegalInput = false; break;
                        case ConsoleKey.A: pos[0]++; pos[2] = 3; illegalInput = false; break;
                        case ConsoleKey.S: pos[1]--; pos[2] = 0; illegalInput = false; break;
                        case ConsoleKey.D: pos[0]--; pos[2] = 1; illegalInput = false; break;
                    }
                    if (!illegalInput) { RenderMap(loadedMap, pos, screen, noclip); screen.Refresh(); }
                    if (key.Key == ConsoleKey.Enter) {
                        RenderMap(loadedMap, pos, screen, noclip);
                        Interface textbuh = new Interface();
                        textbuh.text = new string[] { "     DEBUG MENU     ", "","Set Map   ", "Set X_Pos   ", "Set Y_Pos   ", "Set Scale   ", "Set Width   ", "Set Collision   " };
                        textbuh.DebugBox(screen, currentMap, pos, map, noclip);
                    }
                }
            }
        }
        static void RenderMap(string[] loadedMap, int[] pos, Screen screen, bool noclip) {
            for (int y = 0; y < loadedMap.Length; y++) {
                for (int x = 0; x < loadedMap[y].Length; x++) {
                    screen.Draw(loadedMap[y][x], x + pos[0], y + pos[1]);
                }
            }
            if (!noclip) {
                if (screen.screen[screen.height][screen.width] == '█' || screen.screen[screen.height][screen.width] == '▒') {
                    switch (pos[2]) {
                        case 2: pos[1]--; RenderMap(loadedMap, pos, screen, noclip); break;
                        case 3: pos[0]--; RenderMap(loadedMap, pos, screen, noclip); break;
                        case 0: pos[1]++; RenderMap(loadedMap, pos, screen, noclip); break;
                        case 1: pos[0]++; RenderMap(loadedMap, pos, screen, noclip); break;
                    }
                } else if (screen.screen[screen.height][screen.width] != '▓') {
                    screen.DrawPlayer();
                }
            } else {
                screen.DrawPlayer();
            }
        }
    }
    public class Interface {
        public string[] text = null!;
        public char[] box = {'╔','╗','╚','╝','═','║',' '};
        public void TextBox(Screen screen, byte posX, byte posY) {
            string L = text.OrderByDescending(s => s.Length).First();
            screen.Draw(box[0],posX,posY);
            for (int i = 0; i < L.Length; i++) {
                screen.Draw(box[4], posX+1+i, posY);
            }
            screen.Draw(box[1], posX+L.Length+1, posY);
            screen.Refresh();
            Thread.Sleep(50);
            for (int j = 0; j < text.Length; j++) {
                screen.Draw(box[5], posX, posY+1+j);
                for (int i = 0; i < L.Length; i++) {
                    screen.Draw(box[6], posX + 1 + i, posY+1+j);
                }
                screen.Draw(box[5], posX + L.Length + 1, posY+1+j);
                screen.Refresh();
                Thread.Sleep(50);
            }
            screen.Draw(box[2], posX, posY+1+text.Length);
            for (int i = 0; i < L.Length; i++) {
                screen.Draw(box[4], posX + 1 + i, posY + 1 + text.Length);
            }
            screen.Draw(box[3], posX + L.Length + 1, posY + 1 + text.Length);
            screen.Refresh();
            Thread.Sleep(50);
            for (int i = 0; i < text.Length; i++) {
                for (int j = 0; j < text[i].Length; j++) {
                    screen.Draw(text[i][j],posX+1+j,posY+1+i);
                    screen.Refresh();
                    Thread.Sleep(50);
                }
            }
            screen.Refresh();
        }
        public void DebugBox(Screen screen, byte currentMap, int[] pos, Map map, bool noclip) {
            string L = text.OrderByDescending(s => s.Length).First();
            byte posX = (byte)screen.screen[0].Length; posX -= (byte)L.Length; posX -= 2;
            byte posY = 0;
            sbyte cursor = 0;
            screen.Draw(box[0], posX, posY);
            for (int i = 0; i < L.Length; i++) {
                screen.Draw(box[4], posX + 1 + i, posY);
            }
            screen.Draw(box[1], posX + L.Length + 1, posY);
            screen.Refresh();
            Thread.Sleep(50);
            for (int j = 0; j < text.Length; j++) {
                screen.Draw(box[5], posX, posY + 1 + j);
                for (int i = 0; i < L.Length; i++) {
                    screen.Draw(box[6], posX + 1 + i, posY + 1 + j);
                }
                screen.Draw(box[5], posX + L.Length + 1, posY + 1 + j);
                screen.Refresh();
                Thread.Sleep(50);
            }
            screen.Draw(box[2], posX, posY + 1 + text.Length);
            for (int i = 0; i < L.Length; i++) {
                screen.Draw(box[4], posX + 1 + i, posY + 1 + text.Length);
            }
            screen.Draw(box[3], posX + L.Length + 1, posY + 1 + text.Length);
            screen.Refresh();
            Thread.Sleep(50);
            for (int i = 0; i < 2; i++) {
                for (int j = 0; j < text[i].Length; j++) {
                    screen.Draw(text[i][j], posX + 1 + j, posY + 1 + i);
                }
            }
            ConsoleKeyInfo key;
            do {
                for (int i = 2; i < 8; i++) {
                    if (cursor + 2 == i) {
                        string highlight = "> ";
                        for (int j = 0; j < highlight.Length; j++) {
                            screen.Draw(highlight[j], posX + 1 + j, posY + 1 + i);
                        }
                    }
                    for (int j = 0; j < text[i].Length; j++) {
                        if (cursor + 2 == i) {
                            screen.Draw(text[i][j], posX + 3 + j, posY + 1 + i);
                        } else {
                            screen.Draw(text[i][j], posX + 1 + j, posY + 1 + i);
                        }
                    }
                }
                screen.Refresh();
                key = Console.ReadKey(true);
                switch (key.Key) {
                    case ConsoleKey.W: cursor--; break;
                    case ConsoleKey.S: cursor++; break;
                }
                if (cursor < 0) {
                    cursor = 0;
                }
                if (cursor > 5) {
                    cursor = 5;
                }
            } while (key.Key != ConsoleKey.Enter);
            posY += (byte)text.Length; posY += 2;
            short value = 0;
            screen.Draw(box[0], posX, posY);
            for (int i = 0; i < L.Length; i++) {
                screen.Draw(box[4], posX + 1 + i, posY);
            }
            screen.Draw(box[1], posX + L.Length + 1, posY);
            screen.Refresh();
            Thread.Sleep(50);
            for (int j = 0; j < 1; j++) {
                screen.Draw(box[5], posX, posY + 1 + j);
                for (int i = 0; i < L.Length; i++) {
                    screen.Draw(box[6], posX + 1 + i, posY + 1 + j);
                }
                screen.Draw(box[5], posX + L.Length + 1, posY + 1 + j);
                screen.Refresh();
                Thread.Sleep(50);
            }
            screen.Draw(box[2], posX, posY + 2);
            for (int i = 0; i < L.Length; i++) {
                screen.Draw(box[4], posX + 1 + i, posY + 2);
            }
            screen.Draw(box[3], posX + L.Length + 1, posY + 2);
            screen.Refresh();
            do {
                string modify = $"Set Value: {value}   ";
                for (int j = 0; j < modify.Length; j++) {
                    screen.Draw(modify[j], posX + 1 + j, posY + 1);
                }
                screen.Refresh();
                key = Console.ReadKey(true);
                switch (key.Key) {
                    case ConsoleKey.A: value--; break;
                    case ConsoleKey.D: value++; break;
                }
            } while (key.Key != ConsoleKey.Enter);
            switch (cursor) {
                case 0: currentMap = (byte)value; break;
                case 1: pos[0] = value; break;
                case 2: pos[1] = value; break;
                case 3: map.mapSizeMultiplier = (byte)value; break;
                case 4: map.mapStretchMultiplier = (byte)value; break;
                case 5: if (value == 0) { noclip = false; } else { noclip = true; } break;
            }
            screen.Refresh();
        }
    }
    public class Map {
        public byte previousMap = 0;
        public byte mapSizeMultiplier = 3;
        public byte mapStretchMultiplier = 2;
        public string[] LoadMap(byte mapPointer) {
            string[] MainMap = {
            "██████████████████████████████████████████████████████████████",
            "█                             ██                             █",
            "█ ███████████████████████████▓██▓███████████████████████████ █",
            "█ █                           ██                           █ █",
            "█ █ ░░░░░░░░░░░░░░░░░░░░░░░░░░██░░░░░░░░░░░░░░░░░░░░░░░░░░ █ █",
            "█ █ ░                         ██                     ░   ░ █ █",
            "█ █ ░ ███████                 ██                     ░     █ █",
            "█ █ ░ █     █                 ██           ▒▒▒   ▒▒▒   ▒▒▒▒█ █",
            "█ █ ░ █     █                 ██          ▒▒▒▒▒ ▒▒▒▒▒ ▒▒▒▒▒█ █",
            "█ █ ░ █     █                 ██          ▒▒▒▒▒ ▒▒▒▒▒ ▒▒▒▒▒█ █",
            "█ █ ░ █     ▓                 ██          ▒▒▒▒▒ ▒▒▒▒▒ ▒▒▒▒▒█ █",
            "█ █ ░ █    ██                 ██           ▒▒▒   ▒▒▒   ▒▒▒▒█ █",
            "█ █ ░ ████▓█                                         ░     █ █",
            "█ █ ░        ████████████████████████████████████    ░░░░░ █ █",
            "█ █ ░        █                                  █        ░ █ █",
            "█ █ ░ ██  ██ █  ██████████████████████████████  █ █████  ░ █ █",
            "█ █ ░        █ ██       ▒     ▒        ▒     ██ █ █      ░ █ █",
            "█ █ ░ ██  ██ █ █     ▒     ▒        ▒  ▒      █ █ █      ░ █ █",
            "█ █ ░        █ █     ▒     ▒     ▒  ▒         █ █ █████  ░ █ █",
            "█ █ ░ ██ ███ █ █        ▒     ▒  ▒            █ █        ░ █ █",
            "█ █ ░        █ █     ████████████████████     █ █ █████  ░ █ █",
            "█ █ ░ ██  ██ █ █    ██▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒██    █ █ █   █  ░ █ █",
            "█ █ ░        █ █   ▒█▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒█    █ █ █████  ░ █ █",
            "█ █ ░ ██  ██ █ █  ▒▒█▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒█    █ █ █   █  ░ █ █",
            "█ █ ░        █ █   ▒█▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒█    █ █        ░ █ █",
            "█ █ ░ ██  ██ █ █▒   █▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒██    █ █ █ ███  ░ █ █",
            "█ █ ░        █ █▒▒  █▒▒▒▒▒▒▒▒▒▒▒▒▒▒██████▒    █ █ ██     ░ █ █",
            "█ █ ░░░░░░░░ █ █▒▒▒ █▒▒▒▒▒▒▒▒▒▒▒▒▒██▒▒▒▒▒     █ █ █░░░░░░░ █ █",
            "█ █        ░ █ █▒▒  █▒▒▒▒▒▒▒▒████▒█▒          █ █ ░        █ █",
            "█ ████████ ░ █ █▒   █▒▒▒▒▒▒▒▒█  █▒█▒         ▒█ █ ░ ████████ █",
            "█ ████████ ░ █ █    ██▒▒▒▒▒▒▒█  █▒█▒    ▒▒▒▒▒██ █ ░ ████████ █",
            "█ █        ░ █ █    ▒██████▒▒█  █▒█▒   ▒██████  █ ░        █ █",
            "█ █ ░░░░░░░░ █ █   ▒      ██▒█  █▒█▒  ▒██       █ ░░░░░░░░ █ █",
            "█ █ ░        █ █    ▒      ███  █▒█▒  ▒██       █        ░ █ █",
            "█ █ ░        █ ██      ▒▒   ██  █▒█▒   ▒██████  █        ░ █ █",
            "█ █ ░        █  ██      ▒   ██  █▒█▒    ▒▒▒▒▒██ █        ░ █ █",
            "█ █ ░        █   ████████   ██  █▒█▒         ▒█ █        ░ █ █",
            "█ █ ░        █  ██      ▒   ██  █▒█▒          █ █        ░ █ █",
            "█ █ ░        █ ██      ▒▒   ██  █▒██▒▒▒▒▒     █ █        ░ █ █",
            "█ █ ░        █ █    ▒▒     ███  █▒▒██████▒    █ █        ░ █ █",
            "█ █ ░        █ █    ▒     ████  █▒▒▒▒▒▒▒██    █ █        ░ █ █",
            "█ █ ░        █ █    ██████████▓▓██████████    █ █        ░ █ █",
            "█ █ ░        █ █          ░ ░ ░░ ░ ░          █ █        ░ █ █",
            "█ █ ░        █ █            ░ ░░ ░            █ █        ░ █ █",
            "█ █ ░        █ █          ░ ░ ░░ ░ ░          █ █        ░ █ █",
            "█ █ ░        █ ██           ░ ░░ ░           ██ █        ░ █ █",
            "█ █ ░        █  ██████████████▓▓██████████████  █        ░ █ █",
            "█ █ ░        █                ░░                █        ░ █ █",
            "█ █ ░        █████████████████▓▓█████████████████        ░ █ █",
            "█ █ ░                         ░░                         ░ █ █",
            "█ █ ░     █████ █████ █████   ░░ ███████████████████████ ░ █ █",
            "█ █ ░     █   █ █   █ █   █   ░░ ▒ ▒ ▒ ▒ ▒ ▒ ▒ ▒ ▒ ▒ ▒ ▒ ░ █ █",
            "█ █ ░     █   █ █   █ █   █   ░░░░░░░░░░░░░░░░░░░░░░░░░░░░ █ █",
            "█ █ ░     █   █ █   █ █   █   ░░ ▒ ▒ ▒ ▒ ▒ ▒ ▒ ▒ ▒ ▒ ▒ ▒ ░ █ █",
            "█ █ ░     ██▓██ ██▓██ ██▓██   ░░ ███████████████████████ ░ █ █",
            "█ █ ░       ░     ░     ░     ░░                         ░ █ █",
            "█ █ ░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░ █ █",
            "█ █                                                        █ █",
            "█ ██████████████████████████████████████████████████████████ █",
            "█                                                            █",
            "██████████████████████████████████████████████████████████████",
            };
            string[] selectedMap;
            switch(mapPointer) {
                case 0: selectedMap = MainMap; break;
                default: selectedMap = MainMap; break;
            }
            selectedMap = SizeCorrection(selectedMap);
            return selectedMap;
        }
        private string[] SizeCorrection(string[] originalMap) {
            string[] modifiedMap = new string[originalMap.Length * mapSizeMultiplier];
            for (int y2 = 0; y2 < mapSizeMultiplier; y2++) {
                for (int y = 0; y < originalMap.Length; y++) {
                    for (int x = 0; x < originalMap[y].Length; x++) {
                        for (int x2 = 0; x2 < mapSizeMultiplier * mapStretchMultiplier; x2++) {
                            modifiedMap[y2 + (y * mapSizeMultiplier)] += originalMap[y][x];
                        }
                    }
                }
            }
            return modifiedMap;
        }
    }
    public class Screen {
        public byte width = 0;
        public byte height = 0;
        public string[] screen = null!;
        public void Generate() {
            width = (byte)Console.WindowWidth;
            height = (byte)Console.WindowHeight;
            bool OddW = false; bool OddH = false;
            if (width % 2 == 0) { width--; OddW = true; }
            height -= 2; if (height % 2 == 0) { height--; OddH = true; }
            screen = new string[height];
            for (byte y = 0; y < height; y++) {
                for (byte x = 0; x < width; x++) {
                    screen[y] += "▒";
                }
            }
            if (OddW) { width++; width /= 2; width--; } else { width /= 2; }
            if (OddH) { height++; height /= 2; height--; } else { height /= 2; }
        }
        public void Refresh() {
            string display = "";
            for (byte i = 0; i < screen.Count(); i++) {
                display += screen[i] + "\n";
            }
            Console.SetCursorPosition(0, 1);
            Console.Write(display);
        }
        public void Draw(char symb, int x, int y) {
            if (x >= screen[0].Length || x < 0) {
            } else if (y > screen.Length - 1 || y < 0) {
            } else {
                string line = "";
                for (int i = 0; i < screen[y].Length; i++) {
                    if (i == x) {
                        line += symb;
                    } else {
                        line += screen[y][i];
                    }
                }
                screen[y] = line;
            }
        }
        public void DrawPlayer() {
            Draw('O', width,height);
        }
    }
}