using System;

public interface IHoverUi
{
    event Action<bool> OnHover;

    void SetChildHover(bool setting);

    bool SetAsChild(IHoverUi hoverUi);
}
