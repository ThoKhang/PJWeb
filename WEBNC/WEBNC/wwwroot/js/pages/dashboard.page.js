// wwwroot/js/pages/dashboard.page.js

const moneyVN = (v) => (v || 0).toLocaleString("vi-VN") + " ₫";

async function loadSummary() {
    try {
        const res = await fetch("/api/admin/dashboard/summary");
        if (!res.ok) return;

        const d = await res.json();

        const elRevenue = document.getElementById("txtTodayRevenue");
        const elOrders = document.getElementById("txtTodayOrders");
        const elUsers = document.getElementById("txtNewUsers");
        const elProducts = document.getElementById("txtActiveProducts");

        if (elRevenue) elRevenue.textContent = moneyVN(d.todayRevenue);
        if (elOrders) elOrders.textContent = `${d.todayOrders} đơn`;
        if (elUsers) elUsers.textContent = `${d.newUsers7Days} người`;
        if (elProducts) elProducts.textContent = `${d.totalActiveProducts} sản phẩm`;
    } catch (err) {
        console.error("loadSummary error", err);
    }
}

async function loadRevenueChart() {
    const canvas = document.getElementById("chartRevenue");
    if (!canvas) return;

    try {
        const res = await fetch("/api/admin/dashboard/revenue-by-month");
        if (!res.ok) return;
        const d = await res.json();

        // Chart.js được load global từ layout/section Scripts
        new Chart(canvas, {
            type: "line",
            data: {
                labels: d.labels,
                datasets: [
                    {
                        label: "Doanh thu (₫)",
                        data: d.values,
                        tension: 0.35,
                        fill: true,
                        borderWidth: 2,
                    },
                ],
            },
            options: {
                plugins: { legend: { display: false } },
                scales: {
                    y: {
                        beginAtZero: true,
                        ticks: {
                            callback: (v) => v.toLocaleString("vi-VN") + " ₫",
                        },
                    },
                },
            },
        });
    } catch (err) {
        console.error("loadRevenueChart error", err);
    }
}

async function loadOrderStatusChart() {
    const canvas = document.getElementById("chartOrders");
    const summaryBox = document.getElementById("orderStatusSummary");
    if (!canvas) return;

    try {
        const res = await fetch("/api/admin/dashboard/order-status-ratio");
        if (!res.ok) return;
        const arr = await res.json();

        const labels = arr.map((x) => x.status);
        const values = arr.map((x) => x.count);

        new Chart(canvas, {
            type: "doughnut",
            data: {
                labels,
                datasets: [
                    {
                        data: values,
                        borderWidth: 1,
                    },
                ],
            },
            options: {
                plugins: {
                    legend: {
                        position: "bottom",
                        labels: { boxWidth: 14, font: { size: 12 } },
                    },
                },
                cutout: "60%",
            },
        });

        if (summaryBox) {
            const total = values.reduce((s, v) => s + v, 0) || 1;
            summaryBox.innerHTML = arr
                .map((x) => {
                    const percent = ((x.count * 100) / total).toFixed(1);
                    return `
                        <div class="d-flex align-items-center justify-content-between mb-1">
                            <span>${x.status}</span>
                            <span class="fw-bold">${percent}%</span>
                        </div>
                    `;
                })
                .join("");
        }
    } catch (err) {
        console.error("loadOrderStatusChart error", err);
    }
}

async function loadRecentOrders() {
    const tbody = document.querySelector("#tblRecentOrders tbody");
    if (!tbody) return;

    try {
        const res = await fetch("/api/admin/dashboard/recent-orders");
        if (!res.ok) return;
        const arr = await res.json();

        tbody.innerHTML = arr
            .map((o) => {
                const dateStr = o.date
                    ? new Date(o.date).toLocaleDateString("vi-VN")
                    : "";

                let statusClass = "status";
                if (o.status === "Completed") statusClass += " ok";
                else if (o.status === "Processing") statusClass += " warn";
                else statusClass += " bad";

                return `
                    <tr>
                        <td>${o.code}</td>
                        <td>${o.customer ?? ""}</td>
                        <td>${dateStr}</td>
                        <td>${moneyVN(o.total)}</td>
                        <td><span class="${statusClass}">${o.status}</span></td>
                    </tr>
                `;
            })
            .join("");
    } catch (err) {
        console.error("loadRecentOrders error", err);
    }
}

async function initDashboard() {
    await Promise.all([
        loadSummary(),
        loadRevenueChart(),
        loadOrderStatusChart(),
        loadRecentOrders(),
    ]);
}

document.addEventListener("DOMContentLoaded", () => {
    initDashboard();

    const btnRefresh = document.getElementById("btnRefreshDashboard");
    if (btnRefresh) {
        btnRefresh.addEventListener("click", () => {
            initDashboard();
        });
    }
});
