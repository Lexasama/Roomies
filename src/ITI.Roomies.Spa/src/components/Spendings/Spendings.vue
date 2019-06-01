<template>
  <div>
    <div id="app">
      <canvas id="planet-chart"></canvas>
    </div>
  </div>
</template>

<script>
import Chart from "chart.js";
import planetChartData from "../Spendings/Chart-Data.js";

export default {
  data() {
    return {
      planetChartData: planetChartData
    };
  },

  async mounted() {
    debugger;
    this.createChart("planet-chart", this.planetChartData);
  },

  methods: {
    createChart(chartId, chartData) {
      const ctx = document.getElementById(chartId);
      const myChart = new Chart(ctx, {
        type: chartData.type,
        data: chartData.data,
        options: chartData.options
      });
    }
  }
};
</script>

