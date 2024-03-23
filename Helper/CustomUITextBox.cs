using Terraria.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.GameContent.UI.Elements;
using ReLogic.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace QuantumCommunicator.Helper;
public class CustomUITextBox : UITextBox
{
    public static string text = string.Empty;
    private bool isBackspaceHeld = false;
    private double lastBackspaceTime = 0;
    private bool showCursor = true;
    private int cursorBlinkerCount = 0;
    private const int cursorBlinkerTime = 20; // Time in milliseconds for cursor to blink
    private const double backspaceRepeatRate = 5; // Rate in milliseconds for repeating backspace
    public static bool focused = false;
    private Vector2 textDisplayPosition = Vector2.Zero;

    public CustomUITextBox(string _text, float textScale = 1, bool large = false) : base(_text, textScale, large)
    {
        text = _text;
        ShowInputTicker = false;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        HandleKeyboardInput();

        cursorBlinkerCount++;
        if (cursorBlinkerCount > cursorBlinkerTime)
        {
            showCursor = !showCursor;
            cursorBlinkerCount = 0;
        }
    }

    public string GetText(){
        return text;
    }
    public override void LeftClick(UIMouseEvent evt)
    {
        base.LeftClick(evt);
        focused = true;
        Main.blockInput = true;
    }

    private void HandleKeyboardInput()
    {
        if (!focused) return;
        bool shiftPressed = Main.keyState.IsKeyDown(Keys.LeftShift) || Main.keyState.IsKeyDown(Keys.RightShift);

        var keys = Main.keyState.GetPressedKeys();
        bool backspaceProcessed = false;

        foreach (var key in keys)
        {
            if (key == Keys.Back)
            {
                backspaceProcessed = true;
                if (!isBackspaceHeld || (Main.GameUpdateCount - lastBackspaceTime) >= backspaceRepeatRate)
                {
                    if (text.Length > 0)
                    {
                        text = text[..^1];
                        lastBackspaceTime = Main.GameUpdateCount;
                        AdjustTextDisplayPosition();
                    }
                    isBackspaceHeld = true;
                }
            }
            else if (Main.oldKeyState.IsKeyUp(key)) // Only process newly pressed keys
            {
                if (key == Keys.Escape)
                {
                    Unfocus();
                }
                else
                {
                    char keyChar = GetCharFromKey(key, shiftPressed);
                    if (keyChar != '\0')
                    {
                        text += keyChar;
                        AdjustTextDisplayPosition();
                    }
                }
            }
        }

        if (!backspaceProcessed)
        {
            isBackspaceHeld = false;
        }

        // SetText(text);

        Main.oldKeyState = Main.keyState; // Update the old state
    }


    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        base.DrawSelf(spriteBatch);
        string displayText = showCursor && focused ? text + "|" : text;
        // Assuming textDisplayPosition.Y is already set appropriately
        Vector2 position = GetInnerDimensions().Position() + textDisplayPosition;
        Utils.DrawBorderString(spriteBatch, displayText, position, Color.White);
    }
    private void AdjustTextDisplayPosition(){
        DynamicSpriteFont font = FontAssets.MouseText.Value;
        var textSize = font.MeasureString(text);
        if(textSize.X > Width.Pixels - 50){
            textDisplayPosition.X = Width.Pixels - 50 - textSize.X;
        } else {
            textDisplayPosition.X = 0;
        
        }
    }
    private static char GetCharFromKey(Keys key, bool shiftPressed)
    {
        // This method should convert Keys to char.
        // For simplicity, this example handles only a small subset of keys.
        // You would need to expand this to handle more keys, including letters, numbers, and special characters.
        if (key >= Keys.A && key <= Keys.Z)
        {
            return shiftPressed ? (char)('A' + (key - Keys.A)) : (char)('a' + (key - Keys.A));
        }

        return key switch
        {
            Keys.D0 => shiftPressed ? ')' : '0',
            Keys.D1 => shiftPressed ? '!' : '1',
            Keys.D2 => shiftPressed ? '@' : '2',
            Keys.D3 => shiftPressed ? '#' : '3',
            Keys.D4 => shiftPressed ? '$' : '4',
            Keys.D5 => shiftPressed ? '%' : '5',
            Keys.D6 => shiftPressed ? '^' : '6',
            Keys.D7 => shiftPressed ? '&' : '7',
            Keys.D8 => shiftPressed ? '*' : '8',
            Keys.D9 => shiftPressed ? '(' : '9',
            Keys.OemPeriod => shiftPressed ? '>' : '.',
            Keys.OemComma => shiftPressed ? '<' : ',',
            Keys.OemQuestion => shiftPressed ? '?' : '/',
            Keys.OemSemicolon => shiftPressed ? ':' : ';',
            Keys.OemQuotes => shiftPressed ? '"' : '\'',
            Keys.OemTilde => shiftPressed ? '~' : '`',
            Keys.OemOpenBrackets => shiftPressed ? '{' : '[',
            Keys.OemCloseBrackets => shiftPressed ? '}' : ']',
            Keys.OemPipe => shiftPressed ? '|' : '\\',
            Keys.OemMinus => shiftPressed ? '_' : '-',
            Keys.OemPlus => shiftPressed ? '+' : '=',
            Keys.Space => ' ',
            _ => '\0',// No valid char for the key
        };
    }
    public static void Unfocus()
    {
        focused = false;
        Main.blockInput = false; // Optional: Unblock game input when not focused
    }
}
