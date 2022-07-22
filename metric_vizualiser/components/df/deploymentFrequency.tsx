import * as Plot from "@observablehq/plot";
import { COLORS } from "const";
import * as d3 from "d3";
import { useEffect, useRef } from "react";
import { IDeploymentFrequency } from "types";
type DeploymentFrequencyProps = {
  data?: IDeploymentFrequency;
  months: number;
  size: {width: number, height: number};
};
export function DeploymentFrequency({
  data,
  months,
  size,
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
        background: COLORS.PAPER,
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
        Plot.rectY(transformedData, {
          x: "date",
          y: "count",
          fill: COLORS.BLUE,
          fillOpacity: 0.75,
          strokeOpacity: 0.15,
          stroke: COLORS.BLUE,
          interval: interval,
        }),
        Plot.ruleY([0]),
      ],
      width: size.width,
      height: size.height,
    });
    // @ts-ignore
    headerRef.current.append(chart);
    return () => chart.remove();
  }, [data, months, interval, size]);
  return (
    <div style={{ paddingLeft: "10px" }} ref={headerRef}>
      <h3 style={{ color: COLORS.WHITE}}>{"Deployment Frequency"}</h3>
    </div>
  );
}
