let minusBtn;
let plusBtn;
let quantityInput;

export function initQuantity() {
    minusBtn = document.querySelector(".minus");
    plusBtn = document.querySelector(".plus");
    quantityInput = document.querySelector(".qty");

    if (!minusBtn || !plusBtn || !quantityInput) {
        console.warn("Không tìm thấy nút quantity");
        return;
    }

    quantityInput.value = Number(quantityInput.value) || 1;

    minusBtn.onclick = () => {
        let val = Number(quantityInput.value) || 1;
        if (val > 1) {
            quantityInput.value = val - 1;
        }
    };

    plusBtn.onclick = () => {
        let val = Number(quantityInput.value) || 1;
        quantityInput.value = val + 1; 
    };
}

export function getQuantity() {
    return Number(quantityInput?.value || 1);
}
