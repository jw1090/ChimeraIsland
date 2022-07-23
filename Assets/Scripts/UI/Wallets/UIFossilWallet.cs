public class UIFossilWallet : UIWalletBase
{
    public override void UpdateWallet()
    {
        if (_initialized == false)
        {
            return;
        }

        _walletText.text = _currencyManager.Fossils.ToString();
    }
}