public class UIEssenceWallet : UIWalletBase
{
    public override void UpdateWallet()
    {
        if (_initialized == false)
        {
            return;
        }

        _walletText.text = _currencyManager.Essence.ToString();
    }
}