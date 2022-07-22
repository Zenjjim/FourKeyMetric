import * as Plot from "@observablehq/plot";
import { COLORS } from "const";
import { useEffect, useRef } from "react";
import { ILeadTimeChange } from "types";
import { getDateOfWeek } from "utils";
type LeadTimeChangeProps = {
  data?: ILeadTimeChange;
  months: number;
  size: {width: number, height: number};
};

export function LeadTimeChange({ data, months, size }: LeadTimeChangeProps) {
  const headerRef = useRef(null);
  useEffect(() => {
    if (data === undefined) {
      return;
    }
    const getMontlyLeadTimeChange = (data: ILeadTimeChange) =>
      data.monthlyLeadTimeChange.map((d) => ({
        date: new Date(d.key.yearNumber, d.key.monthNumber),
        median: d.median / 3600,
      }));
    const getWeeklyLeadTimeChange = (data: ILeadTimeChange) =>
      data.weeklyLeadTimeChange.map((d) => ({
        date: getDateOfWeek(d.key.weekNumber, d.key.yearNumber),
        median: d.median / 3600,
      }));

    const interval =
      months < 4 ? data.weeklyLeadTimeChange : data.monthlyLeadTimeChange;
    const transformedDataMedian =
      months < 4
        ? getWeeklyLeadTimeChange(data)
        : getMontlyLeadTimeChange(data);

    const transformedData = data.changes.map((d) => ({
      date: new Date(d.startTime * 1000),
      time: (d.finishTime - d.startTime) / 3600,
      prSize: d.prSize,
      nrOfCommits: d.nrOfCommits
    }));

    const chart = Plot.plot({
      style: {
        background: COLORS.PAPER,
      },
      y: {
        grid: true,
        label: "Hours",
      },
      color: {
        type: "diverging",
        scheme: "burd",
        range: ["red", "blue"],
        interpolate: "hcl",
      },
      marks: [
        Plot.ruleY([0]),
        Plot.line(transformedDataMedian, {
          x: "date",
          y: "median",
          curve: "basis",
          marker: "circle",
          stroke: COLORS.BLUE,
          opacity: 1,
        }),

        Plot.dot(transformedData, {
          x: "date",
          y: "time",
          r: "nrOfCommits",
          fill: COLORS.BLUE,
          fillOpacity: 0.75,
          interval: interval,
        }),
      ],
      width: size.width,
      height: size.height,
    });
    // @ts-ignore
    headerRef.current.append(chart);
    return () => chart.remove();
  }, [data, months, size]);

  return (
    <div style={{ paddingLeft: "10px" }} ref={headerRef}>
      <h3 style={{ color: COLORS.WHITE }}>{"Median Lead Time for Changes"}</h3>
    </div>
  );
}
