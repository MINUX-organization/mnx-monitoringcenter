-- Table: public.presets

-- DROP TABLE IF EXISTS public.presets;

CREATE TABLE IF NOT EXISTS public.presets
(
    id uuid NOT NULL,
    memory_clock integer NOT NULL,
    core_clock integer NOT NULL,
    power_limit integer NOT NULL,
    critical_temperature integer NOT NULL,
    fan_speed integer NOT NULL,
    gpu_name text COLLATE pg_catalog."default" NOT NULL,
    user_id bigint NOT NULL,
    CONSTRAINT pk_presets PRIMARY KEY (id)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.presets
    OWNER to postgres;
-- Index: ix_presets_user_id

-- DROP INDEX IF EXISTS public.ix_presets_user_id;

CREATE INDEX IF NOT EXISTS ix_presets_user_id
    ON public.presets USING btree
    (user_id ASC NULLS LAST)
    TABLESPACE pg_default;