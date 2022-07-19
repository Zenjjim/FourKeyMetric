import * as Plot from "@observablehq/plot";
import * as d3 from "d3";
import { useEffect, useRef } from "react";
import { IDeploymentFrequency } from "types";
type DeploymentFrequencyProps = { data?: IDeploymentFrequency; months: number };
export function DeploymentFrequency({
  data,
  months,
}: DeploymentFrequencyProps) {
  const headerRef = useRef(null);

  const interval =
    months < 1 ? d3.utcDay : months < 3 ? d3.utcWeek : d3.utcMonth;

  useEffect(() => {
    if (data === undefined) {
      return;
    }

    const transformedData = data.weeklyDeployments.map((d) => ({
      date: new Date(d.yearNumber, d.monthNumber - 1, d.dayNumber),
      count: d.deploymentsInBucket.length,
    }));
    const chart = Plot.plot({
      style: {
        background: "transparent",
      },
      y: {
        grid: true,
      },
      color: {
        type: "diverging",
        scheme: "burd",
        range: ["red", "blue"],
        interpolate: "hcl",
      },
      marks: [
        Plot.ruleY([0]),
        Plot.rectY(transformedData, {
          x: "date",
          y: "count",
          fill: "steelblue",
          fillOpacity: 0.75,
          interval: interval,
        }),
      ],
    });
    // @ts-ignore
    headerRef.current.append(chart);
    return () => chart.remove();
  }, [data, months, interval]);

  return <div ref={headerRef}></div>;
}
