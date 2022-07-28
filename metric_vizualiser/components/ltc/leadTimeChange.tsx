import { COLORS } from "const";
import * as d3 from "d3";
import {
  CartesianGrid,
  ComposedChart,
  Line,
  ResponsiveContainer,
  Scatter,
  Tooltip,
  XAxis,
  YAxis,
  ZAxis,
} from "recharts";
import { ILeadTimeChange } from "types";
import { getDateOfWeek } from "utils";
type LeadTimeChangeProps = {
  data: ILeadTimeChange;
  months: number;
};

export function LeadTimeChange({ data, months }: LeadTimeChangeProps) {
  const getMontlyLeadTimeChange = (data: ILeadTimeChange) =>
    data.monthlyLeadTimeChange.map((d) => ({
      date: new Date(d.key.yearNumber, d.key.monthNumber - 1).getTime(),
      median: d.median / 3600,
    }));
  const getWeeklyLeadTimeChange = (data: ILeadTimeChange) =>
    data.weeklyLeadTimeChange.map((d) => ({
      date: getDateOfWeek(d.key.weekNumber, d.key.yearNumber).getTime(),
      median: d.median / 3600,
    }));
  const transformedDataMedian =
    months < 6 ? getWeeklyLeadTimeChange(data) : getMontlyLeadTimeChange(data);

  const prData = data.changes.map((d) => ({
    date: new Date(d.startTime * 1000).getTime(),
    median: (d.finishTime - d.startTime) / 3600,
    prSize: d.prSize,
    nrOfCommits: d.nrOfCommits,
  }));

  const CustomTooltip = ({
    active,
    payload,
  }: {
    active: boolean | undefined;
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    payload: Array<any>;
  }) => {
    if (
      active &&
      payload.length > 0 &&
      // eslint-disable-next-line no-prototype-builtins
      !payload[0].payload.hasOwnProperty("prSize")
    ) {
      return (
        <div
          style={{
            backgroundColor: COLORS.PAPER,
            padding: "5px",
            borderRadius: "10px",
          }}
        >
          <p className="label">{`Time to Change : ${
            Math.round(payload[0].payload.median) + " Hours"
          }`}</p>
          <p className="label">{`Date : ${new Date(
            payload[0].payload.date
          ).toDateString()}`}</p>
        </div>
      );
    }
    return null;
  };

  return (
    <ResponsiveContainer height="100%" width="100%">
      <ComposedChart margin={{ top: 50, right: 50, bottom: 30, left: 30 }}>
        <text
          dominantBaseline="central"
          fill={COLORS.WHITE}
          textAnchor="middle"
          x={200}
          y={20}
        >
          <tspan fontSize="20" fontWeight="bolder">
            Median Lead Time to Change
          </tspan>
        </text>

        <XAxis
          dataKey="date"
          domain={["dataMin", "dataMax"]}
          label={{
            value: "Date",
            dx: 0,
            dy: 25,
            fill: COLORS.WHITE,
            opacity: 0.75,
          }}
          opacity={0.75}
          stroke={COLORS.WHITE}
          tickFormatter={d3.timeFormat("%d %B")}
          type="number"
          xAxisId={1}
        />
        <YAxis
          dataKey="median"
          label={{
            value: "Hours",
            angle: -90,
            dx: -30,
            dy: 0,
            fill: COLORS.WHITE,
            opacity: 0.75,
          }}
          opacity={0.75}
          stroke={COLORS.WHITE}
          type="number"
          yAxisId={1}
        />
        <Scatter
          data={prData}
          fill={COLORS.BLUE}
          fillOpacity={0.75}
          isAnimationActive={false}
          xAxisId={1}
          yAxisId={1}
          zAxisId={1}
        ></Scatter>
        <Line
          data={transformedDataMedian}
          dataKey="median"
          dot={true}
          isAnimationActive={false}
          stroke={COLORS.BLUE}
          strokeWidth={3}
          type="monotone"
          xAxisId={1}
          yAxisId={1}
        />
        <CartesianGrid opacity={0.25} stroke={COLORS.WHITE} vertical={false} />
        <Tooltip
          content={<CustomTooltip active={undefined} payload={[]} />}
          cursor={{ fill: "rgba(255, 255, 255, 0.1)" }}
        />
        <ZAxis dataKey="prSize" range={[0, 4000]} zAxisId={1} />
      </ComposedChart>
    </ResponsiveContainer>
  );
}
