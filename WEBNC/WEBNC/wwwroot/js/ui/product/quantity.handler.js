export function initQuantity() {

    const minusBtn = document.querySelector('.minus');
    const plusBtn = document.querySelector('.plus');
    const quantityInput = document.querySelector('.qty');

    if (minusBtn && plusBtn && quantityInput) {

        minusBtn.addEventListener("click", () => {
            let val = Number(quantityInput.value);
            if (val > 1) quantityInput.value = val - 1;
        });

        plusBtn.addEventListener("click", () => {
            let val = Number(quantityInput.value);
            quantityInput.value = val + 1;
        });

    } else {
        console.warn("Không tìm thấy .minus .plus hoặc .qty");
    }
}

export function getQuantity() {
    return Number(document.querySelector('.qty')?.value || 1);
}
