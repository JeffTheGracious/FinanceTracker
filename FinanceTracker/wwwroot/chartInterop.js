let chart;

// Create a new bar chart
export function renderBarChart(canvasId, labels, values, label) {
    const canvas = document.getElementById(canvasId);
    if (!canvas) {
        console.error(`Canvas '${canvasId}' not found.`);
        return;
    }

    const ctx = canvas.getContext('2d');
    if (!ctx) {
        console.error("Failed to get canvas rendering context.");
        return;
    }

    // Destroy any existing chart instance (prevents overlay issues)
    if (chart) {
        chart.destroy();
    }

    chart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels,
            datasets: [
                {
                    label,
                    data: values,
                    backgroundColor: 'rgba(54, 162, 235, 0.6)',
                    borderColor: 'rgba(54, 162, 235, 1)',
                    borderWidth: 1
                }
            ]
        },
        options: {
            responsive: true,
            scales: {
                y: { beginAtZero: true }
            }
        }
    });
}

// Update existing chart with new data
export function updateBarChart(labels, values, label) {
    if (!chart) return;

    chart.data.labels = labels;
    chart.data.datasets[0].data = values;
    chart.data.datasets[0].label = label;
    chart.update();
}
