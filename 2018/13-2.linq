<Query Kind="Program" />

TrackType[,] track;
List<Cart> carts;
int width = 0;
int height = 0;
List<int> crashedCarts = new List<int>();

void Main()
{
    Setup();
    //Print();
    while (true)
    {
        foreach (var cart in carts.OrderBy(c => c.Y).ThenBy(c => c.X))
        {
            if (crashedCarts.Contains(cart.Id)) continue;
            TrackType nextTrack = TrackType.None;
            switch(cart.Bearing)
            {
                case Bearing.Up:
                    cart.Y -= 1;
                    nextTrack = track[cart.Y, cart.X];
                    break;
                case Bearing.Down:
                    cart.Y += 1;
                    nextTrack = track[cart.Y, cart.X];
                    break;
                case Bearing.Left:
                    cart.X -= 1;
                    nextTrack = track[cart.Y, cart.X];
                    break;
                case Bearing.Right:
                    cart.X += 1;
                    nextTrack = track[cart.Y, cart.X];
                    break;
            }
            if (nextTrack == TrackType.Intersection) cart.IntersectionsPassed = cart.IntersectionsPassed + 1;
            cart.Bearing = CalculateNewBearing(cart.Bearing, nextTrack, cart.IntersectionsPassed);

            foreach (var anyCart in carts)
            {
                if (carts.Where(c => !crashedCarts.Contains(c.Id)).Count(c => c.X == anyCart.X && c.Y == anyCart.Y) > 1)
                {
                    crashedCarts.AddRange(carts.Where(c => !crashedCarts.Contains(c.Id)).Where(c => c.X == anyCart.X && c.Y == anyCart.Y).Select(c => c.Id));
                    
                }
            }
        }
        if (crashedCarts.Count == carts.Count - 1)
        {
            Cart remainingCart = carts.SingleOrDefault(c => !crashedCarts.Contains(c.Id));
            Console.WriteLine("{0},{1}", remainingCart.X, remainingCart.Y);
            return;
        }
    }
}

Bearing CalculateNewBearing(Bearing bearing, TrackType trackType, int intersectionsPassed)
{
    Turn turn;
    if (intersectionsPassed % 3 == 0) turn = Turn.Right;
    else if (intersectionsPassed % 3 == 1) turn = Turn.Left;
    else turn = Turn.Straight;
    switch (bearing)
    {
        case Bearing.Up:
            if (trackType == TrackType.Intersection)
            {
                if (turn == Turn.Left) return Bearing.Left;
                if (turn == Turn.Right) return Bearing.Right;
                if (turn == Turn.Straight) return Bearing.Up;
            }
            if (trackType == TrackType.LTRCorner) return Bearing.Left;
            if (trackType == TrackType.RTLCorner) return Bearing.Right;
            return Bearing.Up;
        case Bearing.Down:
            if (trackType == TrackType.Intersection)
            {
                if (turn == Turn.Left) return Bearing.Right;
                if (turn == Turn.Right) return Bearing.Left;
                if (turn == Turn.Straight) return Bearing.Down;
            }
            if (trackType == TrackType.LTRCorner) return Bearing.Right;
            if (trackType == TrackType.RTLCorner) return Bearing.Left;
            return Bearing.Down;
        case Bearing.Left:
            if (trackType == TrackType.Intersection)
            {
                if (turn == Turn.Left) return Bearing.Down;
                if (turn == Turn.Right) return Bearing.Up;
                if (turn == Turn.Straight) return Bearing.Left;
            }
            if (trackType == TrackType.LTRCorner) return Bearing.Up;
            if (trackType == TrackType.RTLCorner) return Bearing.Down;
            return Bearing.Left;
        case Bearing.Right:
            if (trackType == TrackType.Intersection)
            {
                if (turn == Turn.Left) return Bearing.Up;
                if (turn == Turn.Right) return Bearing.Down;
                if (turn == Turn.Straight) return Bearing.Right;
            }
            if (trackType == TrackType.LTRCorner) return Bearing.Down;
            if (trackType == TrackType.RTLCorner) return Bearing.Up;
            return Bearing.Right;
    }
    
    throw new Exception();
}

void Setup()
{
    string inputString = @"                                    /----------------------------------------------------------------------\                  /------------\          ;        /----------\  /-------------+------------------------------------------------\                     |                  |            |          ;      /-+----------+--+-------------+--\                                             |  /-----------\      |                  |            |          ;      | |          |  |             |  |                     /-----------------------+--+-----------+------+------------------+------------+-------\  ;      | |          |  |             |  |                     |          /------------+--+-----------+------+---------------\  |            |       |  ;    /-+-+----------+--+-------------+--+---------------------+-\        |     /------+--+-----------+------+---------------+\ |            |       |  ; /--+-+-+----------+--+-------------+--+---\                 | |        |     |      |  |           |      |               || |            |       |  ; |  | | | /--------+--+-------------+--+\  |                 | |        |     |      |  |           |      |               || |  /---------+-----\ |  ; |  | | | |     /--+--+----->-------+--++--+----------------\| |        |     |      |  |           |      |      /--------++-+--+---------+-\   | |  ; |  | | | |     |  |  |             |  ||  | /<-------------++-+-\ /----+-----+------+--+-----------+------+------+--------++-+--+\        | |   | |  ; |  | | | |     |  |  |            /+--++--+-+--------------++-+-+-+----+---->+------+--+-----------+------+------+--------++-+--++-----\  | |   | |  ; |  | | | |     |  |  |            ||  ||  | |              || |/+-+----+-----+------+--+-----------+------+\     |        || |  ||     |  | |   | |  ; |  | | | |     |  | /+------------++--++--+-+--------------++-+++-+-\  |     |      |  |           |      ||     |        || |  ||     |  | |   | |  ; | /+-+-+-+-----+--+-++------------++--++--+-+--------------++-+++-+-+--+-----+--\   |  |           |      ||     |        || |  ||     |  | |   | |  ; | || | | |     |  | ||            ||  ||  | |              ||/+++-+-+--+-----+--+---+--+-----------+------++-----+--------++-+-\||     |  | |   | |  ; \-++-+-+-+-----+--+-++------------++--++--/ |              |||||| | |/-+-----+--+---+--+-----------+------++-----+-----\  || | |||     |  | |   | |  ;   || | | |     |  | ||   /--------++--++----+--------------++++++-+-++-+-----+--+---+--+-----------+------++--\  |     |  || | |||     |  | |   | |  ;   || | | |     |  | ||   |        ||  ||    |    /---------++++++-+-++-+-----+--+---+--+-----------+------++--+--+-\   |  || | |||     |  | |   | |  ;   || | | |     |/-+-++---+--------++--++----+----+\ /------++++++-+-++-+-----+--+---+--+-----\     |      ||  |  | |   |  || | |||     |  | |   | |  ;   || |/+-+-----++-+<++---+--------++--++----+\   || |      |||||| | || |     |  |   |  |     |     |      ||  |  | |   |  || | |||     |  | |   | |  ;   || ||| |     || | ||   |        ||  ||    ||   || |      |||||| \-++-+-----+--+---+--+-----+-----+------++--+--+-+---+--++-+-++/     |  | |   | |  ;   || ||| |     || | ||   |        ||  ||    ||   || |      ||||||   || |     |  |   |  |     |     |     /++--+--+\|   |  || | ||      |  | |   | |  ;   || |||/+-----++-+-++---+--------++--++----++---++-+------++++++-\ || |     |  |   |  |     |     |    /+++--+--+++---+->++-+\||      |  | |   | |  ;   || |||||     || | ||   | /------++--++----++---++-+------++++++-+-++-+-----+--+---+--+-----+-----+---\||||  |  |||   |  || ||||      |  | |   | |  ;   || ||||\-----++-+-++---+-+------++--+/    ||   || |  /---++++++-+-++-+-----+--+---+--+-----+----\|   |||||  |  |||   |  || ||||      |  | |   | |  ;   || ||||    /-++-+-++---+-+------++--+-----++---++-+--+---++++++-+-++-+--\  |  |/--+--+-----+----++---+++++--+--+++---+--++-++++------+--+-+---+-+-\;   || ||||    | || | ||   | |      ||  |     ||   || |  |   |\++++-+-++-+--+--+--++--+--+-----+----++---+++++--+--+++---+--++-++++------+--+-+---+-/ |;   || ||||    | || | ||/--+-+------++-\| /---++---++-+--+--\| |||| | || |  |  \--++--+--+-----+----++---+++++--+--+++---+--+/ ||||      |  | |   |   |;   || ||||    | || | |\+--+-+------++-++-+---++---++-+--+--++-++++-+-++-+--+-----++--/  | /---+----++---+++++--+--+++---+--+--++++------+--+-+\  |   |;   || ||||    | || | | |  | |      || || |   ||   || |  |  || |||| | || |  |     ||   /-+-+---+----++---+++++--+--+++---+--+\ ||||      |  | |^  |   |;   || ||||    | || | | |  | |      || || |   ||   || |  |  || ||||/+-++-+--+-----++---+-+-+---+----++---+++++--+\ |||   |  || ||||      |  | ||  |   |;   || ||||    | || | | |  | |      |\-++-+---++---++-+--+--++-++++++-++-+--+-----++---+-+-+---+----++---+++/|  || |||   |  || ||||      |  | ||  |   |;   || ||||    | \+-+-+-+--+-+------+--++-+---++---++-+--+--+/ |||||| || |  | /---++---+-+-+---+----++---+++-+--++-+++---+--++-++++------+\ | ||  |   |;   || ||||    |  | | | |  | |      |  || |   ||   ||/+--+--+--++++++-++-+--+-+---++---+-+-+---+----++---+++-+--++-+++---+--++-++++------++-+-++-\|   |;   |\-++++----+--+-+-+-+--+-+------+--++-+---++---++++--+--+--+/|||| || |  | |   ||   | | |   |    ||   ||| |  || |||   |  || ||||      || | || ||   |;   |  |||\----+--+-+-+-+--+-+------+--++-+---++---++++--+--+--+-+++/ || |  | |   ||   | | |   |    ||   ||\-+--++-+/|   |  || ||||      || | || ||   |;   |  |||    /+--+-+-+-+--+-+------+--++-+---++---++++--+--+--+-+++--++-+--+-+---++---+-+-+---+----++---++\ |  || | |   |  || ||||      || | || ||   |;   |  |||    ||  | | | |  | |  /---+--++-+---++---++++--+--+--+-+++--++-+--+-+---++---+-+-+---+----++---+++-+--++-+-+---+--++-++++---\  || | || ||   |;   |  ||| /--++--+-+-+-+--+-+--+---+--++-+---++---++++--+--+--+-+++--++-+--+-+---++\  | | |   |    ||   ||| |  || | |   |  || ||||   |  || | || ||   |;   |  ||| |  ||  | | | |  | | /+---+--++-+---++---++++--+--+--+-+++--++-+--+-+-\ |||  | | |  /+----++---+++-+--++-+-+---+--++-++++---+\ || | || ||   |;   |  ||| |  ||  | | | |  | | ||   |  ||/+---++---++++--+--+--+-+++--++-+--+-+-+-+++--+-+-+--++----++---+++-+-\|| |/+---+--++-++++---++\|| | || ||   |;   |  ||| |  ||  | | | |  | | ||   |  ||||   ||   ||||  |  |  | |||  |\-+--+-+-+-+++--+-+-+--++----++---+++-+-+++-+++---/  || ||||   ||||| | || ||   |;   |  ||| |  ||  | | | |  | | ||   |  |||\---++---++++--+--/  | |||  |  |  | | | |||  | |/+--++----++---+++-+-+++-+++------++-++++-\ ||||| | || ||   |;  /+--+++-+--++--+-+-+-+--+-+-++---+--+++----++---++++--+-----+-+++--+--+--+-+-+\|||  | |||  ||  /-++---+++-+-+++-+++------++-++++-+-+++++-+-++\||   |;  ||  ||| |  ||  | | | |  | | ||   |  |||    ||   |||| /+-----+-+++--+--+--+-+-+++++--+-+++-\|| /+-++---+++-+-+++-+++------++-++++-+-+++++-+-+++++--\|;  ||  ||\-+--++--+-/ | |  | | ||   |  |||   /++---++++-++-----+-+++--+--+--+-+-+++++--+\||| ||| || ||   ||| | ||| |||      || |||| | ||||| | |||||  ||;  ||  ||  |  || /+---+-+--+-+-++---+--+++---+++---++++-++-----+-+++--+--+--+-+-+++++--+++++-+++-++-++---+++-+-+++-+++-----\|| |||| | ||||| | |||||  ||;  ||  ||  |  || ||   | |  | | ||   |  |||   |||   |||| ||     | |||  |  |  | | |||||  ||||| ||| || ||   ||| | ||| |||     ||| |||| | ||||| | |||||  ||;  |\--++--+--++-++---+-+--+-+-++---+--+++---+++---++++-++-----+-+++--+--+--+-+-++/||  ||||| ||| || ||   ||| | ||| |||     ||| |||| | ||||| | |||||  ||;  |   ||/-+--++-++---+-+--+-+-++---+--+++---+++---++++-++-----+-+++--+--+-\| | || ||  ||||| ||| || ||   |||/+-+++-+++-----+++-++++-+-+++++\| |||||  ||;  |   ||| | /++-++---+-+--+-+-++---+--+++---+++---++++-++-----+-+++--+--+-++-+-++-++--+++++-+++-++-++--\||||| ||| |||     ||| |||| | ||||||| |||||  ||;  |   ||| | ||| |\---+-+--+-+-++---+--+++---+++---+/|| ||     | |||  |  | ||/+-++-++--+++++-+++-++-++--++++++-+++-+++-----+++-++++-+\||||||| |||||  ||;  |   ||| | ||| |  /-+-+--+-+-++---+--+++---+++---+-++-++-----+-+++--+--+-++++-++-++--+++++-+++\|| ||  |||||| ||| |\+-----+++-++++-++++/|||| |||||  ||;  |   ||| | ||| |  | | |  | |/++---+--+++---+++---+-++-++-----+-+++\ |  | |||| || ||  ||||| |||||| ||  |||||| ||| | |     ||| ||||/++++-++++-+++++-\||;  |   ||| | ||v |  | | |  | ||||   |  |||   |||   | || ||     | |||| |  | |||| || ||  ||||| |||||| ||  |||||| ||| | |     ||| ||||||||| |||| ||||| |||;  |   ||| | ||| |  | | | /+-++++---+--+++---+++>--+-++-++-----+-++++\|  | |||| || ||  ||||| |||||| ||  |||||| ||| | |     ||| ||||||||| |||| ||||| |||;  |   ||| | ||| |  | | | || ||||   |  |||   |||   | || ||  /--+-++++++--+-++++-++-++--+++++-++++++-++--++++++-+++-+\|     ||| ||||||||| |||| ||||| |||;  |   ||| | ||| |  | | | || ||||   |  |||   |||   | || ||  |  | ||||||  | |||| || ||  ||||| |||||| ||  |||||| ||| |||     ||| ||||||||| |||| ||||| |||;  |   ||| | ||| |  | | | || ||||   |  |||   |||   | || ||  |  | ||||||  | |||| || ||  ||||| |||||| ||  |||||| ||| |||     ||| ||||||||| |||| ||||| |||;  |   ||| | ||| |  | | | || ||||   |  |||   |||   | || ||  |  | ||||||  | |||| || ||  ||||| |||||| ||  |||||| ||| |||     ||| ||||||||| |||| ||||| |||;  |   ||| | ||| |  |/+-+-++-++++---+--+++---+++---+-++-++--+--+-++++++--+-++++-++-++--+++++-++++++-++--++++++\||| |||     ||| ||||||||| |||| ||||| |||;  |   ||| | |v| |  ||| | || ||||   |  |||   |||   | || ||  |  | ||||||  | |||| || ||  ||||| |||||| ||  |||||||||| |||     ||| ||||||||| |||| ||||| |||; /+---+++-+-+++-+--+++-+-++\||||   |  |||   |||   | || ||  |  | ||||||  | |||| || ||  ||||| |||||| ||  |||||||||| |||     ||| ||||||||| |||| ||||| |||; ||   ||| | |||/+--+++-+-+++++++---+--+++---+++---+-++-++--+--+-++++++--+-++++-++-++--+++++-++++++-++\ |||||||||| |||     ||| ||||||||| |||| ||||| |||; ||   ||| | |||||  ||| | |||||||   |  |||   |||   | || ||  |  | ||||||  | |||| || ||  ||||| |||||| ||| |||||||||| |||     ||| ||||||||| |||| ||||| |||; ||   ||| |/+++++--+++-+-+++++++---+--+++---+++---+-++-++--+--+-++++++--+-++++-++-++--+++++-++++++-+++-++++++++++-+++-\   ||| ||||||||| |||| ||||| |||; ||   ||| |||||||  ||| |/+++++++---+--+++--\|||   | || ||  |  | ||||||  | |||| || ||  ||||| |||||| ||| |||||||||| ||| |   ||| ||||||||| |||| |||v| |||; ||   ||| ||||||\--+++-+++++++++---+--+++--++++---+-++-++--+--+-++++++--+-++++-++-++--+++++-++++++-+++-++++++++++-+++-+---/|| ||||||||| |||| ||||| |||; ||   ||| ||||||   ||| |||||||||  /+--+++--++++---+-++-++--+--+-++++++--+\|||| || ||  ||||| |||||| ||| |||||||||| \++-+----++-+++++++++-++++-/|||| |||; ||   ||| ||||||   ||| |||||||||  ||  |||  ||||   | || ||  \--+-++++++--++++++-++-++--+++++-++++++-+++-++++++++++--/| |    || ||||||||| ||||  |||| |||; ||   ||| ||||||   ||\-+++++++++--++--+++--++++---+-++-++-----+-+++++/  |||||| || ^|  ||||| |||||| ||| ||||||||||   | |    || ||||||||| ||||  |||| |||; ||/--+++-++++++---++--+++++++++--++--+++--++++---+-++-++-----+-+++++---++++++-++-++-\||||| |||||| ||| ||||||||||   | |    || ||||||||| ||||  |||| |||; |||  ||| ||||||   ||  |||||||||  ||  |||  ||||   |/++-++-----+-+++++---++++++-++-++-++++++-++++++-+++-++++++++++\  | |    || ||||||||| ||||  |||| |||; |||  \++-++++++---++--+++++++++--++--+/|  ||||/--++++-++-----+-+++++---++++++-++-++-++++++-++++++\||| |||||||||||  | |    || ||||||||| ||||  |||| |||; |||   || ||||||  /++--+++++++++--++--+-+--+++++--++++-++-----+-+++++--<++++++-++-++-++++++\|||||||||| |||||||||||  | |    || ||||||||| ||||  |||| |||; |||   || ||||||  ||| /+++++++++--++--+-+--+++++--++++-++-----+-+++++-\ |||||| || || |||\+++++++++++/| |||||||||||  | |    || ||||||||| ||||  |||| |||; |||   || ||||||  ||| ||||||||||  ||  | |  ||\++--++++-++-----+-+/||| | |||||| || || ||| ||||||||||| | |||||||||||  | |    || ||||||||| ||||  |||| |||; |||   \+-++++++--+++-++++++++++--++--+-+--++-/|  \+++-++-----+-+-+++-+-++++++-++-++-+++-+++++++++++-+-+++++++++++--/ |    || ||||||||| ||||  |||| |||; |||    | ||||||  ||| ||||||||||  ||  | |  ||  |   ||| ||     | | ||| | |||||| || || ||| \++++++++++-+-+++++++++++----+----++-+++++/||| ||||  |||| |||; |||    | ||||||  ||| ||||||||||  ||  | |  ||  |   ||| ||     | | ||| | |||||\-++-++-+++--++++++++++-+-+++++++++++----+----++-+++++-+++-+/||  |||| |||; |||    | ||||||  ||| ||||||||||  ||  | |  ||  |   ||| ||     | | ||| |/+++++--++-++-+++--++++++++++-+-+++++++++++----+--\ || ||||| ||| | ||  |||| |||; |||    | ||||||  ||\-++++++++++--++--+-+--++--+---+++-++-----+-+-+++-+++++++--++-++-+++--++++++++++-+-++++++/||||    |  | || ||||| ||| | ||  |||| |||; |||    | ||||||  ||  ||||||||||  || /+-+--++--+--\||| ||     | | \++-+++++++--++-++-+++--++++++++++-+-++++++-++/|  /-+--+-++\||||| ||| | ||  |||| |||; |||    | ||||||  ||  ||||||||||  || || |  |\--+--++++-++-----+-+--++-+++++++--++-++-++/  |||||||||| | |||||| || |  | |  | |||||||| ||| | ||  |||| |||; |||    | ||||\+--++--++++++++++--++-++-+--+---+--++++-++-----+-+--++-+++++/|  || || ||   |||||||||| | |||||| || |  | |  | |||||||| ||| v ||  |||| |||; |||    | |||| |  ||  ||||||||||  || || |  |   |  |||| ||   /-+-+--++-+++++-+--++-++-++---++++++++++-+-++++++-++-+--+-+-\| |||||||| ||| | ||  |||| |||; |||    | |||| |/-++--++++++++++--++-++-+--+---+\ |||| ||   | | | /++-+++++-+--++-++-++--\|||||||||| | |||||| || |  | | || |||||||| ||| | ||  |||| |||; |||   /+-++++-++-++--++++++++++--++\|| |  |   || |\++-++---+-+-+-+++-+++++-+--++-++-++--+++++++++++-+-++++++-++-/  | | || |||\++++-+++-+-+/  |||v |||; |||   || |||| || ||  ||||||||||  ||||| |  |   || | || ||   | | \-+++-+++++-+--++-++-++--+++++++++++-+-+++++/ ||    | | || ||| |||| ||| | |   |||| |||; |\+---++-++++-++-++--++++++++++--+++++-+--+---++-+-++-++---+-+---+++-+++++-+--+/ || ||  ||||||||||| | |||||  ||    | | || ||| |||| ||| | |   |||| |||; | |   || |||| || ||  ||||||||||  ||||| |  |   || | || \+---+-+---+++-+++++-+--+--++-++--+++/||||||| | |||||  ||    | | || ||| |||| ||| | |   |||| |||; | |   || |||| || ||  ||||||||||  ||||| |  |   || | ||  |   | |   ||| ||||| |  |  \+-++--+++-+++++++-+-+++++--++----+-+-++-+++-++++-+++-+-+---++++-++/; | |   || |||| || ||  ||||||||||  ||||| |  |   || | \+--+---+-+---+++-+++++-+--+---+-++--+++-+++++++-+-+++++--++----+-+-++-+++-++++-+++-+-+---++/| || ; | |   || |||| || ||  ||||\+++++--+++++-+--+---++-+--+--+---+-+---+++-+++++-+--+---+-++--+++-+++++++-+-+++++--+/    | | || ||| |||| ||| | |   || | || ;/+-+---++-++++-++-++--++++-+++++--+++++\|  |   || |  |  \---+-+---+++-+++++-+--+---+-++--+++-++++++/ | |||||  |     | | || ||| |||| ||| | |   || | || ;|| |   || |||| || ||  |||| |||||  |||\+++--+---++-/  |      | |  /+++-+++++-+--+---+\||  ||| ||||||  |/+++++--+-----+-+-++-+++-++++-+++-+\|   || | || ;|| |   || |||| \+-++--++++-+++++--+++-+++--+---++----+------+-+--++++-+++++-+--+---++++--+++-++++++--/|||\++--+-----+-+-++-+++-/|\+-+++-+++---++-/ || ;|| |   |\-++++--+-++--++++-+++++--+++-+++--+---++----+------+-+--++++-++++/ |  |   ||||  ||| ||||||   ||| ||  |     | | || |||  | | ||| |||   ||   || ;|| |   |  ||||  | || /++++-+++++--+++-+++--+---++----+-----\| |  |||| ||||  |  |   ||||  ||| ||||\+---+++-++--+-----+-+-++-+++--+-+-+++-+++---+/   || ;|| |   |  ||||  | || ||||| |||||  ||| |||  |   ||    |     || |  |||| ||||  |  |  /++++--+++-++++-+---+++-++--+-----+-+-++-+++--+-+-+++-+++---+----++\;|| |   |  ||||  | || |||\+-+++++--+++-+++--/   ||    |     |\-+--++++-++++--+--+--+++++--+++-++++-+---+++-++--+-----+-+-/| |||  | | ||| |||   |    |||;|| |   |  ||||  | || ||| | |||||  ||| |||      ||    |     |  |  |||| ||||  |  |  |||||  ||| |||| |   ||| ||  |     | |  | |||  | | ||| |||   |    |||;|| |   |  |||| /+-++-+++-+-+++++--+++-+++------++----+-----+--+--++++-++++--+--+--+++++--+++-++++-+---+++-++--+-----+-+\ | |||  | | ||| |||   |    |||;|| |   |  |||| || \+-+++-+-+++++--+++-+++------++----+-----+--+--++++-++++--+--+--+++++--++/ |||| |   ||| ||  |     | || | |||  | | ||| |||   |    |||;|| |   |  |||| ||  | ||| | |||||  ||| |||   /--++----+-----+--+--++++-++++--+--+--+++++--++--++++-+---+++-++--+-----+-++-+-+++--+\| ||| |||   |    |||;|| |   |  |||| ||  | ||| | |||||  ||| |||   |  ||    |     |  |  |||| ||||  |  |  |||||  ||  |||| |   ||| ||  |     | || | |||  ||| ||| |||   |    |||;|| |   |  |||| ||  | ||| | |||||  |\+-+++---+--++----+-----+--+--++++-++++--+--+--+++++--++--++++-+---+++-++--+-----+-++-+-+++--+++-+++-/||   |    |||;|| |   |  |||| ||  | ||| | |||||  | | |||   |  ||    |     |  \--++++-++++--+--+--+++++--++--++++-+---+++-++--+-----+-++-+-+++--/|| |||  ||   |    |||;|| | /-+--++++-++--+-+++-+-+++++-\| | |||   |  ||    |     |   /-++++-++++--+--+--+++++--++--++++-+---+++-++--+----\| || | |||   || |||  ||   |    |||;|| | | |  ||\+-++--+-+++-+-+++++-++-+-+++---+--++----+-----+---+-++++-++++--+--+--+++++--++--++++-+---+/| ||  |    || || | |||   || |||  ||   |    |||;|| | | |  || | ||  | ||| | ||||| || | ||\---+--++----+-----+---+-++++-++++--+--+--+++++--++--++++-+---+-+-++--/    || || | |||   || |||  ||   |    |||;|| | | |  || | ||  | ||| | ||||| || | ||    |  ||    |     |   | |||| ||||  |  |  |||||  ||  |||| |   | | ||       || || | |||   || |||  ||   |    |||;|| | | |  || | ||  | ||| | ||||| || | ||    |  ||    |     |   | |||| ||\+--+--+--+++++--++--++++-+---+-+-++-------++-++-+-/||   || |||  ||   |    |||;|| | | |  || | ||  | \++-+-+++++-++-+-++----+--++----+-----/   | |||| || |  |  |  |||||/-++--++++-+---+-+-++-------++-++-+--++---++-+++--++--\|    |||;|| | | |  || | ||/-+--++-+-+++++-++-+-++----+--++----+\        | |||| || |  | /+--++++++-++--++++-+---+-+-++-------++-++-+--++---++-+++--++--++--\ |||;|| | | |  || | ||| |  || | ||||| || | ||    |  ||    ||        | |||| || |  | ||  |||||| ||  |||| |   | | ||       || || |  ||   || |||  ||  ||  | |||;|| | | |  || | ||| |  || | ||||| || | ||    |  ||    ||        | \+++-++-+--+-++--++/||| ||  |||| |   | | |\-------++-++-+--++---++-+++--+/  ||  | |||;|| | | |  || | ||| |  || | ||||| || | ||    | /++----++--------+--+++-++-+\ | ||  || ||| ||  |||| | /-+-+-+--------++-++-+--++\  || |||  |   ||  | |||;|| | | |  || | ||| |  || | |||||/++-+-++----+-+++----++-----\  |  ||| || || | ||  || ||| ||  |||\-+-+-+-+-+--------++-++-+--+++--++<+++--+---++--+-+/|;|| | | |  || | ||| |  || | |||||||| | ||    | |||    ||     |  |  ||| || || | ||  || |\+-++--+++--+-+-+-+-+--------++-++-+--/||  || |||  |   ||  | | |;|| | | |  || | ||| |  || \-++++++++-+-++----+-+++----++-----+--+--++/ || || | ||  || | | ||  |||  | | | | |   /----++-++-+---++--++-+++--+---++--+-+\|;|| | | |  || | ||| |  ||   |\++++++-+-++----+-+++----++-----+--+--++--++-++-+-++--++-+-+-++--+++--+-+-+-/ |   |    || || |   ||  || |||  |   ||  | |||;|| | | |  || | ||| |  ||   | |||||| | ||    | |||    ||     |  |  \+--++-++-+-++--++-+-+-/|  |||  | | |   |   |    || || |   ||  || |||  |   ||  | |||;|| | | | /++-+-+++-+--++---+-++++++-+-++----+\|||    ||     |  |   |  || || | \+--++-+-+--+--+++--+-+-+---+---+----++-++-+---++--++-+++--+---++--/ |||;|| | | | ||| | ||| |  ||   | ||\+++-+-++----+++++----++-----+--+---+--++-++-+--+--++-+-+--+--+++--+-+-+---+---+----++-++-+---++--++-+/|  |   ||    |||;|| | | | ||| | ||| |  |\---+-++-+++-+-/|    |||||    \+-----+--+---+--++-++-+--+--++-+-+--+--+/|  | | |   |   |    || || |   ||  |\-+-+--+---++----/||;|| | | | ||| | ||| |  |    | || ||| |  |    |||||     |     |  |   |  || || |/-+--++-+-+--+--+-+--+-+-+---+--\|    || || |   ||  |  | |  |   ||     ||;|| | \-+-+++-+-+++-+--+----+-++-+/| |  |    |||||     |     |  |   |  || || || |  \+-+-+--+--+-+--+-+-+---+--++----++-++-+---++--+--+-+--+---++-----+/;|| |   | ||| | ||| |  |    | || | | |  |    |||||     |     |  |   |  || || || |   | | |  |  | |  | | |   |  |v    || || |   ||  |  | |  |   ||     | ;|| |   | ||| \-+++-+--+----+-++-+-+-+--+----+++++-----+-----+--+---+--++-++-++-+---+-+-+--+--+-+--+-+-+---/  ||    |\-++-+---/|  |  | |  |   ||     | ;|\-+---+-+++---+++-+--+----/ \+-+-+-+--+----+++++-----+-----+--+---/  || || || |   | | |  |  | |  | | |      ||    |  || |    |  |  | |  |   ||     | ;|  |   | |||   ||| |  |       | | | |  |    |||||     |     |  |      |^ || || |   | | |  |  \-+--+-+-+------++----+--++-+----+--+--+-/  |   ||     | ;|  |   | \++---+++-+--+-------+-+-+-+--+----+/|\+-----+-----+--+------++-++-++-+---+-+-+--+----+--/ | |      ||    |  || |    |  |  |    |   ||     | ;|  |   |  ||   ||| |  |       | \-+-+--+----+-+-+-----+-----/  |      |\-++-++-+---+-+-+--+----+----+-+------++----+--++-/    |  |  |    |   ||     | ;|  |   \--++---+++-+--+-------+---+-/  |    | | |     |        |      |  || || |   | | |  \----+----+-+------++----+--++------+--+--+----+---+/     | ;|  |      ||   ||| |  \-------+---+----+----+-+-+-----+--------+------/  || || |   | | |       |    \-+------++----+--++------/  |  |    |   |      | ;|  |      ||   ||\-+----------+---+----+----+-+-+-----/        |         || || |   | | |       |      |      |\----+--++---------+--+----+---+------/ ;|  |      ||   ||  |          |   \----+----+-+-+--------------+---------/| || |   | | |       |      |      |     |  ||         |  |    |   |        ;|  \------++---++--+----------+--------+----+-+-+--------------+----------+-++-+---+-/ |       |      |      |     |  ||         |  |    |   |        ;|         |\---++--+----------+--------+----+-+-+--------------+----------+-++-+---+---+-------+------+------+-----+--/|         |  |    |   |        ;|         |    ||  |          |        |    | | |              |          | || |   |   |       |      |      |     |   |         |  |    |   |        ;|         |    ||  |          |        |    | | |              |          | |\-+---+---+-------+------+------/     |   |         |  |    |   |        ;\---------+----++--+----------+--------/    \-+-+--------------+----------+-+--+---+---+-------+------+------------+---+---------/  |    |   |        ;          |    |\--+----------+---------------+-/              |          | \--+---+---+-------+------+------------+---+------------/    |   |        ;          |    |   |          |               \----------------+----------/    |   |   |       |      |            |   |                 |   |        ;          \----+---+----------+--------------------------------+---------------+---/   |       |      |            |   |                 |   |        ;               |   |          |                                \---------------+-------+-------+------+------------/   |                 |   |        ;               |   |          \------------------------------------------------/       \-------+------+----------------+-----------------+---/        ;               \---+---------------------------------------------------------------------------+------+----------------/                 |            ;                   \---------------------------------------------------------------------------/      \----------------------------------/            ";
    string[] inputLines = inputString.Split(';');
    width = inputLines[0].Count();
    height = inputString.Count(c => c == ';') + 1;
    track = new TrackType[height, width];
    carts = new List<Cart>();
    int y = 0;
    int cartId = 1;
    foreach (var inputLine in inputLines)
    {
        for (int x = 0; x < inputLine.Length; x++)
        {
            switch (inputLine[x])
            {
                case ' ':
                    track[y, x] = TrackType.None;
                    break;
                case '|':
                    track[y, x] = TrackType.UpDown;
                    break;
                case '-':
                    track[y, x] = TrackType.LeftRight;
                    break;
                case '\\':
                    track[y, x] = TrackType.LTRCorner;
                    break;
                case '/':
                    track[y, x] = TrackType.RTLCorner;
                    break;
                case '+':
                    track[y, x] = TrackType.Intersection;
                    break;
                case 'v':
                    track[y, x] = TrackType.UpDown;
                    carts.Add(new Cart { Id = cartId, X = x, Y = y, Bearing = Bearing.Down });
                    cartId++;
                    break;
                case '^':
                    track[y, x] = TrackType.UpDown;
                    carts.Add(new Cart { Id = cartId, X = x, Y = y, Bearing = Bearing.Up });
                    cartId++;
                    break;
                case '<':
                    track[y, x] = TrackType.LeftRight;
                    carts.Add(new Cart { Id = cartId, X = x, Y = y, Bearing = Bearing.Left });
                    cartId++;
                    break;
                case '>':
                    track[y, x] = TrackType.LeftRight;
                    carts.Add(new Cart { Id = cartId, X = x, Y = y, Bearing = Bearing.Right });
                    cartId++;
                    break;
            }
        }
        y++;
    }
}

public class Cart
{
    public int Id { get; set;}
    public int X { get; set; }
    public int Y { get; set; }
    public Bearing Bearing { get; set; }
    public int IntersectionsPassed { get; set; }
}

public enum Bearing
{
    Up,
    Down,
    Left,
    Right
}

public enum Turn
{
    Left,
    Right,
    Straight
}

public enum TrackType
{
    None,
    UpDown,
    LeftRight,
    LTRCorner,
    RTLCorner,
    Intersection
}

public void Print()
{
    for (int y = 0; y < height; y++)
    {
        for (int x = 0; x < width; x++)
        {
            List<Cart> cartsHere = carts.Where(c => c.X == x && c.Y == y).ToList();
            if (cartsHere.Count > 0)
            {
                if (cartsHere.Count > 1) Console.Write('X');
                else if (cartsHere[0].Bearing == Bearing.Up) Console.Write('^');
                else if (cartsHere[0].Bearing == Bearing.Down) Console.Write('v');
                else if (cartsHere[0].Bearing == Bearing.Left) Console.Write('<');
                else if (cartsHere[0].Bearing == Bearing.Right) Console.Write('>');
            }
            else
            {
                if (track[y, x] == TrackType.None) Console.Write(' ');
                else if (track[y, x] == TrackType.LeftRight) Console.Write('-');
                else if (track[y, x] == TrackType.UpDown) Console.Write('|');
                else if (track[y, x] == TrackType.LTRCorner) Console.Write('\\');
                else if (track[y, x] == TrackType.RTLCorner) Console.Write('/');
                else if (track[y, x] == TrackType.Intersection) Console.Write('+');
            }
        }
        Console.WriteLine();
    }
    Console.WriteLine();
}